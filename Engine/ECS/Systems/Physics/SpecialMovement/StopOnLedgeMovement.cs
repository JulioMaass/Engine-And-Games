using Engine.ECS.Components;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Systems.Physics.GeneralPhysics;
using Engine.ECS.Systems.Physics.SolidPhysics;
using Engine.Types;
using System;

namespace Engine.ECS.Systems.Physics.SpecialMovement;

public class StopOnLedgeMovement : Component
{
    private IntVector2 Pixel => Owner.Position.Pixel;

    // General Physics
    private PhysicsHelperFunctions PhysicsHelperFunctions => Owner.Physics.PhysicsHelperFunctions;

    // Solid Physics
    private SolidCollidingMovement SolidCollidingMovement => Owner.Physics.SolidCollidingMovement;
    private SolidCollisionChecking SolidCollisionChecking => Owner.Physics.SolidCollisionChecking;

    public StopOnLedgeMovement(Entity owner)
    {
        Owner = owner;
    }

    public void MoveToSolidAndStopOnLedges()
    {
        SolidCollidingMovement.MoveToSolidY();
        var xSpeed = GetSpeedLimitedToLedgeDistance();
        SolidCollidingMovement.MoveToSolidX(xSpeed);
    }

    private float GetSpeedLimitedToLedgeDistance()
    {
        var xSpeed = Owner.Speed.X;
        if (!SolidCollisionChecking.IsOnTopOfSolid())
            return xSpeed;

        var ledgeDistance = FindLedgeDistance(xSpeed);
        xSpeed = Math.Clamp(xSpeed, -ledgeDistance, ledgeDistance);
        Owner.Speed.SetXSpeed(xSpeed);
        return xSpeed;
    }

    private int FindLedgeDistance(float xSpeed)
    {
        // TODO: Take hitbox size into account (if floor is smaller than hitbox, entity should fall)
        var xMaxDestiny = PhysicsHelperFunctions.GetMaxedDestinyX(xSpeed); // TODO: Use actual GetPixelDestiny instead of maxed destiny?
        var xDistance = xMaxDestiny - Pixel.X;
        var xDir = Math.Sign(xDistance);

        var collisionDirectionLedge = IntVector2.New(xDir, 1);
        var edgePosition = Owner.CollisionBox.GetEdgePosition(collisionDirectionLedge) + IntVector2.PixelDown + (xDir, 0);

        // Search for ledge
        for (var i = 0; i < Math.Abs(xDistance); i++)
        {
            var distance = xDir * i;
            var pointToTest = edgePosition + (distance, 0);
            if (!SolidCollisionChecking.IsThereSolidAtPoint(pointToTest))
                return Math.Abs(xDir * i);
        }
        return Math.Abs(xDistance);
    }
}
