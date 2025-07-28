using Engine.ECS.Components;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Systems.Physics.GeneralPhysics;
using Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS.Systems.Physics.SolidPhysics;

public class PushAndCarryMovement : Component
{
    // General Physics
    private PhysicsHelperFunctions PhysicsHelperFunctions => Owner.Physics.PhysicsHelperFunctions;
    private FreeMovement FreeMovement => Owner.Physics.FreeMovement;
    // Solid Physics
    private SolidCollisionChecking SolidCollisionChecking => Owner.Physics.SolidCollisionChecking;

    public IntVector2 PreMovePosition { get; private set; }
    public List<Entity> CarriablesOnTop { get; private set; }
    public IntVector2 FrameSpeed => Owner.Position.Pixel - PreMovePosition;

    public PushAndCarryMovement(Entity owner)
    {
        Owner = owner;
    }

    public void SavePreMovePosition()
        => PreMovePosition = Owner.Position.Pixel;

    public void UpdateCarriablesOnTop()
        => CarriablesOnTop = GetCarriablesOnTop();

    public List<Entity> GetCarriablesOnTop()
    {
        var list = new List<Entity>();
        if (Owner.SolidBehavior?.SolidType == SolidType.NotSolid)
            return list;

        list.AddRange(EntityManager.GetAllEntities()
            .Where(entity => SolidCollisionChecking.EntityIsOnTop(entity)));
        return list;
    }

    public void SnapPushablesUp()
    {
        foreach (var entity in EntityManager.GetAllEntities())
        {
            if (entity.SolidBehavior?.SolidInteractionType != SolidInteractionType.StopOnSolids) continue;
            if (entity.SolidBehavior?.SolidType != SolidType.NotSolid) continue;
            if (!entity.CollisionBox.CollidesWithEntityPixel(Owner)) continue;

            var previousPosition = Owner.Position.Pixel - (0, FrameSpeed.Y);
            if (Owner.Physics.EntityCollisionChecking.CollidesWithEntityAtPixel(entity, previousPosition)) continue;

            var travelDistance = entity.CollisionBox.GetCollisionRectangle().Bottom - Owner.CollisionBox.GetCollisionRectangle().Top - 1;
            var dir = Math.Sign(travelDistance);

            for (var i = 0; i < Math.Abs(travelDistance); i++)
            {
                if (entity.Physics.SolidCollisionChecking.CollidesWithAnySolidAtPixel(entity.Position.Pixel + (0, dir), entity.Position.Pixel))
                    break;
                entity.Physics.FreeMovement.MoveInPixelsAndFraction(0, dir);
            }
        }
    }

    public void CarryCarriablesOnTop()
    {
        foreach (var entity in CarriablesOnTop)
            entity.Physics.SolidCollidingMovement.MoveToSolid(FrameSpeed);
    }

    public void ConveyorBeltCarry(int xSpeed)
    {
        foreach (var entity in CarriablesOnTop)
            entity.Physics.SolidCollidingMovement.MoveToSolidX(xSpeed);
    }
}
