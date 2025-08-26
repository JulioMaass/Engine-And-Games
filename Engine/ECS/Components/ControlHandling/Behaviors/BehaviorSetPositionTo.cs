using Engine.ECS.Entities.EntityCreation;
using System;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorSetPositionTo : Behavior
{
    Func<Entity> TargetEntityGetter { get; set; }

    public BehaviorSetPositionTo(Func<Entity> entityGetter)
    {
        TargetEntityGetter = entityGetter;
    }

    public override void Action()
    {
        Owner.Position.Pixel = TargetEntityGetter().Position.Pixel;
    }
}
