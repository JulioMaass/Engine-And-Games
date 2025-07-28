using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.StageHandling;
using Engine.Types;
using System.Collections.Generic;

namespace Engine.ECS.Components.PhysicsHandling;

public class TileDestructor : Component
{
    private Strength Strength { get; set; }

    public TileDestructor(Entity owner, Strength strength)
    {
        Owner = owner;
        Strength = strength;
    }

    public void Update()
    {
        if (Strength == 0)
            return;

        var room = StageManager.CurrentRoom;
        var collisionBox = Owner.CollisionBox.GetTileCollisionRectangle();
        collisionBox.OffsetPosition(-room.PositionInTiles);

        for (var tileY = collisionBox.Top; tileY <= collisionBox.Bottom; tileY++)
        {
            for (var tileX = collisionBox.Left; tileX <= collisionBox.Right; tileX++)
            {
                var position = IntVector2.New(tileX, tileY);
                var tileProperties = room.DebugTiles.GetTilePropertiesAt(position);
                if (CanDestroy(tileProperties))
                    room.DestroyTileAt(position);
            }
        }
    }

    private bool CanDestroy(List<TileProperty> tileProperty)
    {
        if (Strength == Strength.DestroysNone)
            return false;
        if (Strength == Strength.DestroysWeak && tileProperty.Contains(TileProperty.DestructibleWeak))
            return true;
        if (Strength == Strength.DestroysStrong && (tileProperty.Contains(TileProperty.DestructibleStrong) || tileProperty.Contains(TileProperty.DestructibleWeak)))
            return true;
        return false;
    }
}

public enum Strength
{
    DestroysNone,
    DestroysWeak,
    DestroysStrong,
}