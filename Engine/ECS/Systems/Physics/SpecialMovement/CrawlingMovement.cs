using Engine.ECS.Components;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Systems.Physics.GeneralPhysics;
using Engine.ECS.Systems.Physics.SolidPhysics;
using Engine.Types;
using System;

namespace Engine.ECS.Systems.Physics.SpecialMovement;

public class CrawlingMovement : Component
{
    private IntVector2 Pixel => Owner.Position.Pixel;

    // General Physics
    private PhysicsHelperFunctions PhysicsHelperFunctions => Owner.Physics.PhysicsHelperFunctions;

    // Solid Physics
    private SolidCollidingMovement SolidCollidingMovement => Owner.Physics.SolidCollidingMovement;

    public CrawlingMovement(Entity owner)
    {
        Owner = owner;
    }

    public void Crawl()
    {
        if (Owner.Speed.CrawlTurnDirection == 0)
            return;

        if (Owner.Speed.X != 0 && Owner.Speed.Y != 0)
            CrawlDiagonally();
        else
            CrawlStraight();
    }

    private void CrawlDiagonally()
    {
        // TODO: Implement? Or just make it fly diagonal and change state when hitting a wall?
    }

    private void CrawlStraight()
    {
        // TODO: May lose some travel distance when it turns (both inwards and outwards). May want to fix this if crawlers move together, since it's more visible
        Owner.Speed.ApplyMoveSpeedToCurrentVectorSpeed();

        // If it won't touch a solid at the end of movement, turn on edge
        CheckToCrawlOutwards();

        // Move
        var startingSpeed = Owner.Speed.Value;
        SolidCollidingMovement.MoveToSolidY();
        SolidCollidingMovement.MoveToSolidX();

        // If hit a wall, turn
        if ((IntVector2)Owner.Speed.Value == IntVector2.Zero)
        {
            Owner.Speed.SetSpeed(startingSpeed);
            WallTurn();
        }
    }

    private void CheckToCrawlOutwards()
    {
        var movementDirection = GetCrawlDirection();
        var turnDirection = GetEdgeTurnDirection(movementDirection);

        // Check if it's touching a solid
        var turnPixel = Pixel + turnDirection;
        if (!Owner.Physics.SolidCollisionChecking.CollidesWithAnySolidAtPixel(turnPixel, Pixel))
            return;

        // Check if it won't collide with a solid at the end of movement
        var xDestiny = PhysicsHelperFunctions.GetMaxedDestinyX(Owner.Speed.Value.X);
        var yDestiny = PhysicsHelperFunctions.GetMaxedDestinyY(Owner.Speed.Value.Y);
        var destinyPixel = IntVector2.New(xDestiny, yDestiny);
        var turnDestinyPixel = destinyPixel + turnDirection;
        if (Owner.Physics.SolidCollisionChecking.CollidesWithAnySolidAtPixel(turnDestinyPixel, destinyPixel))
            return;

        // If it's colliding with a solid and won't collide with a solid at the end of movement, turn on edge
        Owner.Position.Pixel = Pixel + turnDirection;
        Owner.Physics.SolidCollidingMovement.MoveThroughSolidUntilNotColliding(movementDirection);
        EdgeTurn();
    }

    private IntVector2 GetCrawlDirection()
    {
        var xDir = Math.Sign(Owner.Speed.X);
        var yDir = Math.Sign(Owner.Speed.Y);
        return IntVector2.New(xDir, yDir);
    }

    private void WallTurn()
    {
        if (Owner.Speed.CrawlTurnDirection > 0)
            Owner.Speed.TurnClockwise();
        else
            Owner.Speed.TurnCounterClockwise();
    }

    private void EdgeTurn()
    {
        if (Owner.Speed.CrawlTurnDirection > 0)
            Owner.Speed.TurnCounterClockwise();
        else
            Owner.Speed.TurnClockwise();
    }

    private IntVector2 GetEdgeTurnDirection(IntVector2 movementDirection)
    {
        return Owner.Speed.CrawlTurnDirection > 0
            ? IntVector2.TurnCounterClockwise(movementDirection)
            : IntVector2.TurnClockwise(movementDirection);
    }
}
