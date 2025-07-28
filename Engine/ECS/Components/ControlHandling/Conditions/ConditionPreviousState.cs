using Engine.ECS.Components.ControlHandling.States;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionPreviousState : Condition
{
    private State PreviousState { get; }

    public ConditionPreviousState(State previousState)
    {
        PreviousState = previousState;
    }

    protected override bool IsTrue()
    {
        return Owner.StateManager.PreviousState == PreviousState;
    }
}
