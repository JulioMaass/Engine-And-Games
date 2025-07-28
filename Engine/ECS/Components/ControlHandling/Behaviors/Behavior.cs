using Engine.ECS.Components.ControlHandling.Conditions;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public abstract class Behavior : Component
{
    public virtual List<Condition> Conditions { get; } = new();
    public virtual List<Behavior> Behaviors { get; } = new();

    public abstract void Action();

    public void ExecuteIfConditionsAreMet()
    {
        if (AllConditionsAreTrue())
        {
            Action();
            foreach (var behavior in Behaviors)
                behavior.Action();
        }
    }

    private bool AllConditionsAreTrue()
    {
        return Conditions.All(condition => condition.AllConditionsAreTrue());
    }

    public Behavior AddCondition(Condition condition)
    {
        Conditions.Add(condition);
        condition.Owner = Owner;
        return this;
    }

    public Behavior AddBehavior(Behavior behavior)
    {
        Behaviors.Add(behavior);
        behavior.Owner = Owner;
        return this;
    }
}
