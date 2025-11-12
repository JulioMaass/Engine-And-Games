using Engine.ECS.Components.ControlHandling.SecondaryStates;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Entities.EntityCreation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Engine.ECS.Components.ControlHandling;

public class StateManager : Component
{
    // TODO: Add EngineStates(?) for states like hurt and teleport, that are higher priority than other states, and their cause are external to the entity

    // Override states
    public List<State> OverrideStatesList { get; set; }
    // Commanded states
    private List<State> CommandedStatesQueue { get; set; }
    public State CurrentCommandedState { get; private set; }
    // Automatic states
    public List<State> AutomaticStatesList { get; } = new();
    // General state handling
    public (State State, int InitialFrame) DefaultState { get; set; } // If it needs to run a state and all of them failed to be set, this one will be set
    public State PreviousState { get; private set; }
    public State CurrentState { get; private set; }
    // Secondary states
    private List<SecondaryState> SecondaryStateList { get; set; }
    public SecondaryState CurrentSecondaryState { get; private set; }

    public StateManager(Entity owner)
    {
        Owner = owner;
    }

    public State GetCurrentStateOrDefault() => CurrentState ?? AutomaticStatesList.LastOrDefault();

    public void Update()
    {
        UpdateState();
        UpdateSecondaryState();

        RunState();
        RunSecondaryState();
    }

    private void UpdateState()
    {
        OverrideStateUpdate();
        if (OverrideStatesList != null &&
            OverrideStatesList.Contains(CurrentState))
            return;

        CommandedStateUpdate();
        if (CurrentCommandedState != null)
            return;

        AutomaticStateUpdate();
        DefaultStateUpdate();
    }

    private void DefaultStateUpdate()
    {
        if (CurrentState != null)
            return;
        SetCurrentState(DefaultState.State, DefaultState.InitialFrame);
#if DEBUG
        if (CurrentState == null)
            Debugger.Break(); // No state was set, add a default one?
#endif
    }

    private void OverrideStateUpdate()
    {
        if (OverrideStatesList == null)
            return;

        foreach (var state in OverrideStatesList)
        {
            if (CheckToSetState(state))
                return;
            if (CanKeepState(state))
                return;
        }
    }

    public void AddStatesToCommandedStatesQueue(List<State> states)
    {
        CommandedStatesQueue ??= new List<State>();
        CommandedStatesQueue.AddRange(states);
    }

    private void CommandedStateUpdate()
    {
        var stateToCommand = CommandedStatesQueue?.FirstOrDefault();
        if (stateToCommand == null && CurrentCommandedState == null)
            return;

        if (stateToCommand != CurrentCommandedState && stateToCommand != null)
        {
            // Command new state
            if (CheckToSetState(stateToCommand))
                CurrentCommandedState = stateToCommand;
        }
        else
        {
            // Keep command state
            if (CanKeepState(CurrentCommandedState))
                return;

            // Advance command queue
            CommandedStatesQueue.Remove(stateToCommand);
            CurrentCommandedState = null;
            var nextCommandedState = CommandedStatesQueue.FirstOrDefault();
            if (nextCommandedState == null)
                return;
            if (CheckToSetState(nextCommandedState))
                CurrentCommandedState = nextCommandedState;
        }
    }

    private void AutomaticStateUpdate()
    {
        if (AutomaticStatesList.Count == 1 && CurrentState == AutomaticStatesList[0])
            return;

        foreach (var state in AutomaticStatesList)
        {
            if (CheckToSetState(state))
                return;
            if (CanKeepState(state))
                return;
        }
    }

    public void AddFrame()
    {
        if (CurrentState != null)
            CurrentState.Frame++;
    }

    private void RunState()
    {
        CurrentState.UpdateFacing();

        if (CurrentState.Frame == 0)
        {
            CurrentState.StateSettingBehavior();
            CurrentState.AddedStateSettingBehaviors.ForEach(behavior => behavior.ExecuteIfConditionsAreMet());
        }

        if (CurrentState.Frame < CurrentState.StartUpDuration)
            CurrentState.StartUpBehavior();
        else
        {
            CurrentState.Behavior();
            CurrentState.AddedBehaviors.ForEach(behavior => behavior.ExecuteIfConditionsAreMet());
        }

        CurrentState.UpdateAnimationFrame();
    }

    public void PostProcessing()
    {
        UpdateHitbox();

        if (!CurrentState.PostProcessingKeepCondition())
        {
            UpdateState();
            CurrentState.StateSettingBehavior();
            CurrentState.AddedStateSettingBehaviors.ForEach(behavior => behavior.ExecuteIfConditionsAreMet());

            CurrentState.UpdateAnimationFrame();
        }
        RunPostProcessingBehaviors();
    }

    private void RunPostProcessingBehaviors()
    {
        // State setting behaviors
        if (CurrentState.Frame == 0)
        {
            CurrentState.PostProcessingStateSettingBehavior();
            CurrentState.AddedPostProcessingStateSettingBehaviors.ForEach(behavior => behavior.ExecuteIfConditionsAreMet());
        }
        if (CurrentSecondaryState?.Frame == 0)
            CurrentSecondaryState.PostProcessingStateSettingBehavior();

        // Normal behaviors
        CurrentState.PostProcessingBehavior();
        CurrentState.AddedPostProcessingBehaviors.ForEach(behavior => behavior.ExecuteIfConditionsAreMet());
    }

    private bool CheckToSetState(State state)
    {
        var canSetState = Owner.StateManager.CurrentState != state
            && (state.StartCondition() && state.AddedStartConditions.All(condition => condition.AllConditionsAreTrue())) // All main conditions are true
            || state.OptionalStartConditions.Any(condition => condition.AllConditionsAreTrue()); // Or at least one optional condition is true
        if (!canSetState)
            return false;

        SetCurrentState(state);
        return true;
    }

    public void CommandState(State state, int frame = 0) // If a state is commanded it has higher priority over automatic states
    {
        SetCurrentState(state, frame);
        PreviousState = null;
        CurrentCommandedState = state;
    }

    public void CancelCommandedState()
    {
        CurrentCommandedState = null;
    }

    private bool CanKeepState(State state)
    {
        if (Owner.StateManager.CurrentState != state)
            return false;
        if (state.KeepCondition() && state.AddedKeepConditions.All(condition => condition.AllConditionsAreTrue()))
            return true;
        ClearCurrentState();
        return false;
    }

    private void UpdateSecondaryState()
    {
        if (SecondaryStateList == null)
            return;
        if (!CurrentState.AllowSecondaryState)
        {
            CurrentSecondaryState = null;
            return;
        }
        if (CurrentSecondaryState != null)
            CurrentSecondaryState.Frame++;

        foreach (var secondaryState in SecondaryStateList)
        {
            if (CheckToSetSecondaryState(secondaryState))
                break;
            if (CanKeepSecondaryState(secondaryState))
                break;
        }
    }

    public void UpdateHitbox()
    {
        if (Owner.CollisionBox?.Dynamic != true)
            return;

        var intendedHitbox = CurrentState!.CustomHitbox == null
            ? Owner.CollisionBox.DefaultHitbox
            : CurrentState!.CustomHitbox.Value;
        Owner.CollisionBox.SetNewHitboxIfFree(intendedHitbox);
    }

    private void RunSecondaryState()
    {
        if (CurrentSecondaryState?.Frame == 0)
            CurrentSecondaryState.StateSettingBehavior();
        CurrentSecondaryState?.Behavior();
    }

    private bool CheckToSetSecondaryState(SecondaryState secondaryState)
    {
        if (secondaryState.StartCondition())
        {
            CurrentSecondaryState = secondaryState;
            CurrentSecondaryState.Frame = 0;
            return true;
        }
        return false;
    }

    private bool CanKeepSecondaryState(SecondaryState secondaryState)
    {
        return Owner.StateManager.CurrentSecondaryState == secondaryState && secondaryState.KeepCondition();
    }

    public void AddSecondaryState<T>(Action<T> setProperties) where T : SecondaryState, new()
    {
        // DEFAULT VALUES
        var secondaryState = new T();
        secondaryState.Owner = Owner;

        // CUSTOM VALUES
        setProperties?.Invoke(secondaryState);

        SecondaryStateList ??= new List<SecondaryState>();
        SecondaryStateList.Add(secondaryState);
    }

    private void ClearCurrentState()
    {
        if (CurrentState == null)
            return;
        if (CurrentState.Frame != 0)
            CurrentState.AddedStateExitBehaviors.ForEach(behavior => behavior.ExecuteIfConditionsAreMet());
        PreviousState = CurrentState;
        CurrentState = null;
    }

    private void SetCurrentState(State state, int frame = 0)
    {
        ClearCurrentState();
        CurrentState = state;
        CurrentState.Frame = frame;
    }
}
