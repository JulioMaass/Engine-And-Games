using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.States;
using System.Collections.Generic;

namespace Engine.ECS.Components.ControlHandling.StateGrouping;

public class Pattern : Component // Pattern is a list of states that will be executed in order
{
    public List<State> States { get; } = new();
    public virtual List<Condition> Conditions { get; } = new();

    public Pattern(params State[] states)
    {
        States.AddRange(states);
    }

    public void AddCondition(Condition condition)
    {
        condition.Owner = Owner;
        Conditions.Add(condition);
    }
}
