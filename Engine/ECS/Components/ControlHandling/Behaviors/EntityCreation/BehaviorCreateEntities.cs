using Engine.ECS.Entities;
using Engine.Types;
using System;
using System.Collections.Generic;
using Engine.ECS.Components.PositionHandling;

namespace Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;

public class BehaviorCreateEntities : Behavior
{
    public List<Type> EntityTypes { get; } = new ();

    public BehaviorCreateEntities(params Type[] entityTypes)
    {
        EntityTypes.AddRange(entityTypes);
    }

    public override void Action()
    {
        foreach (var entityType in EntityTypes)
            EntityManager.CreateEntityAt(entityType, Owner.Position.Pixel);
    }
}
