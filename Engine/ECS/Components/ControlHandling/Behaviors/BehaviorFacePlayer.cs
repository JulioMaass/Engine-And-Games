using Engine.ECS.Entities;
using System;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorFacePlayer : Behavior
{
    public override void Action()
    {
        if (EntityManager.PlayerEntity == null)
            return;
        var xFacingPlayer = Math.Sign(EntityManager.PlayerEntity.Position.Pixel.X - Owner.Position.Pixel.X);
        Owner.Facing.SetXIfNotZero(xFacingPlayer);
    }
}
