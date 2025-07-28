using Engine.ECS.Components.ControlHandling.States;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionStateFrame : Condition // TODO: This should be a condition combo, not a single condition
{
    private State State { get; }
    private int Frame { get; }

    public ConditionStateFrame(State state, int frame)
    {
        State = state;
        Frame = frame;
    }

    protected override bool IsTrue()
    {
        return Owner.StateManager.CurrentState == State && Owner.StateManager.CurrentState.Frame >= Frame;
    }
}
