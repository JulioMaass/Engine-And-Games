using Engine.ECS.Components;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Systems.Physics.GeneralPhysics;
using Engine.Managers;
using Engine.Managers.StageHandling;
using Engine.Types;
using System.Collections.Generic;

namespace Engine.ECS.Systems.Physics.SolidPhysics;

public class SolidCollisionChecking : Component
{
    private IntVector2 Pixel => Owner.Position.Pixel;

    // General Physics
    private EntityCollisionChecking EntityCollisionChecking => Owner.Physics.EntityCollisionChecking;
    private TileCollisionChecking TileCollisionChecking => Owner.Physics.TileCollisionChecking;

    public SolidCollisionChecking(Entity owner)
    {
        Owner = owner;
    }

    public bool CollidesWithSolidWithPixelSpeed(IntVector2 speed, List<Entity> exceptionList = null)
    {
        var destinyPixel = Pixel + speed;
        return CollidesWithAnySolidAtPixel(destinyPixel, Pixel, exceptionList);
    }

    public bool IsOnTopOfSolid()
    {
        return CollidesWithSolidWithPixelSpeed(IntVector2.PixelDown);
    }

    public bool EntityIsOnTop(Entity entity)
    {
        if (entity == Owner
            || entity.SolidBehavior?.SolidInteractionType != SolidInteractionType.StopOnSolids
            || entity.Speed.Y < 0)
            return false;

        var isColliding = Owner.Physics.EntityCollisionChecking.CollidesWithEntityAtPixel(entity, Owner.Position.Pixel);
        var isCollidingUp = Owner.Physics.EntityCollisionChecking.CollidesWithEntityAtPixel(entity, Owner.Position.Pixel - (0, 1));
        return !isColliding && isCollidingUp;
    }

    public bool IsCollidingWithSolid() =>
        CollidesWithSolidWithPixelSpeed(IntVector2.Zero);

    public bool CollidesWithAnySolidAtPixel(IntVector2 destinyPixel, IntVector2 originPixel, List<Entity> exceptionList = null)
    {
        DebugMode.SolidScanCounter++;
        return CollidesWithAnySolidTileAtPixel(destinyPixel, StageManager.CurrentRoom)
            || CollidesWithAnySolidTopTileAtPixel(destinyPixel, originPixel, StageManager.CurrentRoom)
            || CollidesWithAnySolidEntityAtPixel(destinyPixel, originPixel, exceptionList)
            || CollidesWithAnySolidTopEntityAtPixel(destinyPixel, originPixel, exceptionList);
    }

    private bool CollidesWithAnySolidTileAtPixel(IntVector2 position, Room room)
    {
        return TileCollisionChecking.OverlapsWithTileWithPropertyAtPixel(position, TileProperty.Solid, room);
    }

    private bool CollidesWithAnySolidTopTileAtPixel(IntVector2 destinyPixel, IntVector2 originPixel, Room room)
    {
        if (destinyPixel.Y <= originPixel.Y)
            return false;

        var offsetOrigin = IntVector2.New(destinyPixel.X, originPixel.Y);
        var tileRectangleOrigin = Owner.CollisionBox.GetCollisionRectangleAt(offsetOrigin).RoundDownToTileCoordinate();
        var tileRectangleDestiny = Owner.CollisionBox.GetCollisionRectangleAt(destinyPixel).RoundDownToTileCoordinate();
        tileRectangleOrigin.OffsetPosition(-room.PositionInTiles);
        tileRectangleDestiny.OffsetPosition(-room.PositionInTiles);

        DebugMode.TileCollisionCounter++;
        for (var tileX = tileRectangleDestiny.Left; tileX <= tileRectangleDestiny.Right; tileX++)
        {
            for (var tileY = tileRectangleOrigin.Bottom + 1; tileY <= tileRectangleDestiny.Bottom; tileY++)
            {
                // Collision handling
                var position = IntVector2.New(tileX, tileY);
                var tileType = room.DebugTiles.GetTileTypeAt(position);
                if (TileCollisionChecking.TileTypeHasProperty(tileType, TileProperty.SolidTop))
                    return true;
            }
        }
        return false;
    }

    private bool CollidesWithAnySolidEntityAtPixel(IntVector2 destinyPixel, IntVector2 originPixel, List<Entity> exceptionList = null)
    {
        foreach (var entity in EntityManager.GetAllEntities())
        {
            if (entity == Owner) continue;
            if (exceptionList != null && exceptionList.Contains(entity)) continue;
            if (entity.SolidBehavior?.SolidType != SolidType.Solid) continue;
            if (EntityCollisionChecking.CollidesWithEntityAtPixel(entity, destinyPixel)
                && !EntityCollisionChecking.CollidesWithEntityAtPixel(entity, originPixel))
                return true;
        }
        return false;
    }

    private bool CollidesWithAnySolidTopEntityAtPixel(IntVector2 destinyPixel, IntVector2 originPixel, List<Entity> exceptionList = null)
    {
        foreach (var entity in EntityManager.GetAllEntities())
        {
            if (entity == Owner) continue;
            if (exceptionList != null && exceptionList.Contains(entity)) continue;
            if (entity.SolidBehavior?.SolidType != SolidType.SolidTop) continue;
            if (CollidesWithSolidTopEntityAtPixel(entity, destinyPixel, originPixel))
                return true;
        }
        return false;
    }

    private bool CollidesWithSolidTopEntityAtPixel(Entity entity, IntVector2 destinyPixel, IntVector2 originPixel)
    {
        if (destinyPixel.Y < originPixel.Y) return false;

        var offsetOrigin = IntVector2.New(destinyPixel.X, originPixel.Y);
        var originRectangle = Owner.CollisionBox.GetCollisionRectangleAt(offsetOrigin);
        var destinyRectangle = Owner.CollisionBox.GetCollisionRectangleAt(destinyPixel);

        DebugMode.EntityCollisionCounter++;
        var isColliding = destinyRectangle.Overlaps(entity.CollisionBox.GetCollisionRectangle());
        DebugMode.EntityCollisionCounter++;
        var wasColliding = originRectangle.Overlaps(entity.CollisionBox.GetCollisionRectangle());

        return isColliding && !wasColliding;
    }

    public bool IsThereSolidAtPoint(IntVector2 point)
    {
        return TileCollisionChecking.TileHasPropertyAtPoint(point, TileProperty.Solid);
    }
}
