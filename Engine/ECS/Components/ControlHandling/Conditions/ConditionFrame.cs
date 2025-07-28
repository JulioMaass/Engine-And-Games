using Engine.Types;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionFrame : Condition
{
    private RandomInt Frame { get; }
    public ComparisonType ComparisonType { get; }

    public ConditionFrame(RandomInt frame, ComparisonType comparisonType)
    {
        Frame = frame;
        ComparisonType = comparisonType;
    }

    public ConditionFrame(int frame, ComparisonType comparisonType) // TODO: Make it possible to use int instead of RandomInt (instead of randomizing with no range)
    {
        Frame = new RandomInt(frame, frame);
        ComparisonType = comparisonType;
    }

    protected override bool IsTrue()
    {
        if (ComparisonType == ComparisonType.None)
            throw new System.Exception("ComparisonType not set");
        if (ComparisonType == ComparisonType.Equal)
            return Frame == Owner.StateManager.CurrentState.Frame;
        if (ComparisonType == ComparisonType.Greater)
            return Owner.StateManager.CurrentState.Frame > Frame;
        if (ComparisonType == ComparisonType.Less)
            return Owner.StateManager.CurrentState.Frame < Frame;
        if (ComparisonType == ComparisonType.GreaterOrEqual)
            return Frame <= Owner.StateManager.CurrentState.Frame;
        if (ComparisonType == ComparisonType.LessOrEqual)
            return Frame >= Owner.StateManager.CurrentState.Frame;
        throw new System.Exception("Invalid ComparisonType");
    }
}

public enum ComparisonType
{
    None,
    Equal,
    Greater,
    Less,
    GreaterOrEqual,
    LessOrEqual
}