using Engine.ECS.Components.ControlHandling.States;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionStateNot : Condition
{
    private State State { get; }

    public ConditionStateNot(State state)
    {
        State = state;
    }

    protected override bool IsTrue()
    {
        return Owner.StateManager.CurrentState != State;
    }
}
