using Engine.ECS.Components;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Systems.Physics.GeneralPhysics;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;

namespace Engine.ECS.Systems.Physics.SolidPhysics;

public class SolidCollidingMovement : Component
{
    private IntVector2 Pixel => Owner.Position.Pixel;

    // General Physics
    private PhysicsHelperFunctions PhysicsHelperFunctions => Owner.Physics.PhysicsHelperFunctions;
    private FreeMovement FreeMovement => Owner.Physics.FreeMovement;
    // Solid Physics
    private SolidCollisionChecking SolidCollisionChecking => Owner.Physics.SolidCollisionChecking;

    public SolidCollidingMovement(Entity owner)
    {
        Owner = owner;
    }

    public void MoveToSolid()
    {
        MoveToSolidY();
        MoveToSolidX();
    }

    public void MoveToSolid(Vector2 speed)
    {
        MoveToSolidX(speed.X);
        MoveToSolidY(speed.Y);
    }

    public void MoveToSolidY()
    {
        MoveToSolidY(Owner.Speed.Y);
    }

    public void MoveToSolidY(float speedY)
    {
        if (speedY == 0)
            return;

        var yDestiny = PhysicsHelperFunctions.GetMaxedDestinyY(speedY);
        var destinyPixel = IntVector2.New(Pixel.X, yDestiny);
        if (!SolidCollisionChecking.CollidesWithAnySolidAtPixel(destinyPixel, Pixel))
            FreeMovement.MoveYInPixelsAndFraction(speedY);
        else
            MoveUntilHitSolidY(speedY);
    }

    private void MoveUntilHitSolidY(float speedY)
    {
        var yDir = Math.Sign(speedY);
        var speed = IntVector2.New(0, yDir);
        while (!SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(speed))
            Owner.Position.Pixel += (0, yDir);
        FreeMovement.SetFractionY(0);
        Owner.Speed.SetYSpeed(0);
    }

    public void MoveToSolidX()
    {
        MoveToSolidX(Owner.Speed.X);
    }

    public void MoveToSolidX(float speedX)
    {
        if (speedX == 0)
            return;

        var xDestiny = PhysicsHelperFunctions.GetMaxedDestinyX(speedX);
        var destinyPixel = IntVector2.New(xDestiny, Pixel.Y);
        if (!SolidCollisionChecking.CollidesWithAnySolidAtPixel(destinyPixel, Pixel))
            FreeMovement.MoveXInPixelsAndFraction(speedX);
        else
            MoveToSolidXUntilHitSolid(speedX);
    }

    private void MoveToSolidXUntilHitSolid(float speedX)
    {
        var xDir = Math.Sign(speedX);
        var speed = IntVector2.New(xDir, 0);
        while (!SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(speed))
            Owner.Position.Pixel += (xDir, 0);
        FreeMovement.SetFractionX(0);
        Owner.Speed.SetXSpeed(0); // TODO - BUG - MMDB: May reset speed to 0 even when is being pushed/carried, but speed is an internal/independent value
    }

    public void MoveThroughSolidUntilNotColliding(IntVector2 dir)
    {
        while (SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(IntVector2.Zero))
            Owner.Position.Pixel += dir;
        Owner.Position.Fraction = IntVector2.Zero;
    }
}
