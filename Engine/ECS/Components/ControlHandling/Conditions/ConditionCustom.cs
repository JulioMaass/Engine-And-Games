using System;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionCustom : Condition
{
    private Func<bool> AddedCondition { get; }

    public ConditionCustom(Func<bool> addedCondition)
    {
        AddedCondition = addedCondition;
    }

    protected override bool IsTrue()
    {
        return AddedCondition();
    }
}
