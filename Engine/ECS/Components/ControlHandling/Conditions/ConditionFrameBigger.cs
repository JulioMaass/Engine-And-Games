namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionFrameBigger : Condition
{
    private int Frame { get; }

    public ConditionFrameBigger(int frame)
    {
        Frame = frame;
    }

    protected override bool IsTrue()
    {
        return Owner.StateManager.CurrentState.Frame >= Frame;
    }
}
