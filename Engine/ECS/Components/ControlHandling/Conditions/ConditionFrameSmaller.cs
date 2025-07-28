namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionFrameSmaller : Condition
{
    private int Frame { get; }

    public ConditionFrameSmaller(int frame)
    {
        Frame = frame;
    }

    protected override bool IsTrue()
    {
        return Owner.StateManager.CurrentState.Frame < Frame;
    }
}
