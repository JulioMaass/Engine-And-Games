using Engine.ECS.Components.ControlHandling.Conditions;
using System.Collections.Generic;

namespace Engine.ECS.Components.ControlHandling.StateGrouping;

public class PatternPool : Component // PatternPool is a list of patterns where one of the patterns will be chosen (randomly or based on some conditions)
{
    public List<Pattern> Patterns { get; } = new();
    public virtual List<Condition> Conditions { get; } = new();

    public PatternPool(params Pattern[] patterns)
    {
        Patterns.AddRange(patterns);
    }

    public void AddCondition(Condition condition)
    {
        condition.Owner = Owner;
        Conditions.Add(condition);
    }
}
