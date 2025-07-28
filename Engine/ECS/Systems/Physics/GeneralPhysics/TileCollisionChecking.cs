using Engine.ECS.Components;
using Engine.ECS.Entities.EntityCreation;
using Engine.Main;
using Engine.Managers.StageHandling;
using Engine.Types;

namespace Engine.ECS.Systems.Physics.GeneralPhysics;

public class TileCollisionChecking : Component
{
    public TileCollisionChecking(Entity owner)
    {
        Owner = owner;
    }

    public bool OverlapsWithTileWithPropertyAtPixel(IntVector2 position, TileProperty tileProperty, Room room)
    {
        var tileCollisionRectangle = Owner.CollisionBox.GetTileCollisionRectangleAt(position);
        return room.DebugTiles.IsThereTileWithPropertyInRectangle(tileCollisionRectangle, tileProperty);
    }

    public static bool TileHasPropertyAtPoint(IntVector2 point, TileProperty tileProperty)
    {
        var tilePosition = point.RoundDownDivision(Settings.TileSize) - StageManager.CurrentRoom.PositionInTiles;
        var tileType = StageManager.CurrentRoom.DebugTiles.GetTileTypeAt(tilePosition);
        return DebugTiles.GetTileTypeProperties(tileType).Contains(tileProperty);
    }

    public static bool TileTypeHasProperty(TileType tileType, TileProperty tileProperty)
    {
        return DebugTiles.GetTileTypeProperties(tileType).Contains(tileProperty);
    }
}
