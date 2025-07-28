using Engine.ECS.Components;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Systems.Physics.GeneralPhysics;
using Engine.Helpers;
using Engine.Main;
using Engine.Managers;
using Engine.Managers.StageHandling;
using Engine.Types;

namespace Engine.ECS.Systems.Physics.SpecialMovement;

public class StairHandling : Component
{
    private IntVector2 Pixel => Owner.Position.Pixel;

    // General Physics
    private TileCollisionChecking TileCollisionChecking => Owner.Physics.TileCollisionChecking;

    public StairHandling(Entity owner)
    {
        Owner = owner;
    }

    public bool IsOverlappingWithStair()
    {
        return TileCollisionChecking.OverlapsWithTileWithPropertyAtPixel(Pixel, TileProperty.Stair, StageManager.CurrentRoom);
    }

    public bool CanGrabStair(int grabDistance, int grabTop, int grabBottom)
    {
        var room = StageManager.CurrentRoom;

        var boxTop = (Pixel.Y + grabTop).RoundDownDivision(Settings.TileSize.Height) - room.PositionInTiles.Y;
        var boxBottom = (Pixel.Y + grabBottom - 1).RoundDownDivision(Settings.TileSize.Height) - room.PositionInTiles.Y;
        var boxLeft = (Pixel.X - grabDistance).RoundDownDivision(Settings.TileSize.Width) - room.PositionInTiles.X;
        var boxRight = (Pixel.X + grabDistance - 1).RoundDownDivision(Settings.TileSize.Width) - room.PositionInTiles.X;

        DebugMode.TileCollisionCounter++;
        for (var tileX = boxLeft; tileX <= boxRight; tileX++)
        {
            for (var tileY = boxTop; tileY <= boxBottom; tileY++)
            {
                // Collision handling
                var position = IntVector2.New(tileX, tileY);
                var tileType = room.DebugTiles.GetTileTypeAt(position);
                if (TileCollisionChecking.TileTypeHasProperty(tileType, TileProperty.Stair))
                    return true;
            }
        }
        return false;
    }

    public int GetNearestStairX(int grabDistance, int yOffset)
    {
        // Test in middle, then in front, then behind
        int[] offsets = { 0, grabDistance, -grabDistance };
        DebugMode.TileCollisionCounter++;
        foreach (var offset in offsets)
        {
            var stairX = Pixel.X + Owner.Facing.X * offset;
            var point = IntVector2.New(stairX, Pixel.Y + yOffset);
            if (IsThereStairAtPoint(point))
            {
                return stairX.RoundDownDivision(Settings.TileSize.Width) * Settings.TileSize.Width + Settings.TileSize.Width / 2;
            }
        }
        return Pixel.X;
    }

    private bool IsThereStairAtPoint(IntVector2 point)
    {
        return TileCollisionChecking.TileHasPropertyAtPoint(point, TileProperty.Stair);
    }
}
