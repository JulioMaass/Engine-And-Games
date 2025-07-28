using Engine.ECS.Entities;
using Engine.Types;
using System;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorCreateEntity : Behavior
{
    public Type EntityType { get; set; }
    public IntVector2 RelativePosition { get; set; }

    public BehaviorCreateEntity(Type type) =>
        EntityType = type;

    public BehaviorCreateEntity(Type type, IntVector2 relativePosition)
    {
        EntityType = type;
        RelativePosition = relativePosition;
    }

    public override void Action()
    {
        var position = Owner.Position.Pixel + RelativePosition;
        EntityManager.CreateEntityAt(EntityType, position);
    }
}
