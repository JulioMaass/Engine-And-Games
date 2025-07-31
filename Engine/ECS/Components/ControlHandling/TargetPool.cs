using Engine.ECS.Entities.EntityCreation;
using System.Collections.Generic;

namespace Engine.ECS.Components.ControlHandling;

public class TargetPool : Component
{
    public List<Entity> TargetList { get; } = new();

    public TargetPool(Entity owner)
    {
        Owner = owner;
    }
}
