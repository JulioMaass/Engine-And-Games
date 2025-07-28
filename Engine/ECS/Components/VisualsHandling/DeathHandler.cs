using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Entities.EntityCreation;
using System.Collections.Generic;

namespace Engine.ECS.Components.VisualsHandling;

public class DeathHandler : Component
{
    private List<Behavior> Behaviors { get; } = new();

    public DeathHandler(Entity owner)
    {
        Owner = owner;
    }

    public void AddBehavior(Behavior behavior)
    {
        Behaviors.Add(behavior);
        behavior.Owner = Owner;
    }

    public void RunDeathProcess()
    {
        foreach (var behavior in Behaviors)
            behavior.Action();
    }
}