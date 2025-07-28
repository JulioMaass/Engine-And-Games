namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionFrameEqual : Condition
{
    private int Frame { get; }

    public ConditionFrameEqual(int frame)
    {
        Frame = frame;
    }

    protected override bool IsTrue()
    {
        return Owner.StateManager.CurrentState.Frame == Frame;
    }
}
