using Engine.ECS.Entities;
using System;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionXFacingPlayer : Condition
{
    protected override bool IsTrue()
    {
        var player = EntityManager.PlayerEntity;
        if (player == null)
            return false;
        var directionX = Math.Sign(EntityManager.PlayerEntity.Position.Pixel.X - Owner.Position.Pixel.X);

        return Owner.Facing.X == directionX;
    }
}
