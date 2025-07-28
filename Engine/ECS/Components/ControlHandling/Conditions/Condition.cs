using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public abstract class Condition : Component
{
    private List<Condition> Conditions { get; } = new();
    private bool ReversedCondition { get; set; }

    protected abstract bool IsTrue();

    public Condition AddCondition(Condition condition)
    {
        condition.Owner = Owner;
        Conditions.Add(condition);
        return this;
    }

    public bool AllConditionsAreTrue()
    {
        var isTrue = IsTrue();
        if (ReversedCondition)
            isTrue = !isTrue;
        return isTrue && Conditions.All(condition => condition.AllConditionsAreTrue());
    }

    public Condition Reversed()
    {
        ReversedCondition = true;
        return this;
    }
}
