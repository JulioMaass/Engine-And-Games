using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Managers.StageEditing;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;

namespace Engine.Managers.StageHandling;

public class TileLayout : Layer
{
    public int[,] Layout { get; private set; }
    public Tileset Tileset { get; set; }
    public LayerId LayerId { get; set; }
    public Room Room { get; }
    public IntVector2 Size => Room.SizeInTiles;
    public const int EMPTY = -1; // Empty tile value

    public TileLayout(Room room, Type tilesetType, LayerId layerId)
    {
        Room = room;
        Tileset = StageEditor.TileMode.GetTilesetOfType(tilesetType);
        LayerId = layerId;
    }

    public void SetTileAt(IntVector2 position, int tileId)
    {
        Room.RemoveTileAtLayer(position, LayerId);
        Layout.SetValueAt(position, tileId);
    }

    public int GetValueAt(IntVector2 position)
    {
        DebugMode.TileCheckCounter++;
        return Layout.GetValueAt(position);
    }

    public void CreateTileLayout() =>
        Layout = Extensions.NewArray(Size, EMPTY);

    public bool IsEmpty()
    {
        if (Layout == null)
            return true;
        foreach (var tile in Layout)
            if (tile != EMPTY)
                return false;
        return true;
    }

    public int[,] GetLayoutCopy() =>
        (int[,])Layout?.Clone();

    public void CopyLayout(int[,] layoutToCopy)
    {
        if (layoutToCopy == null)
            return;

        Layout = (int[,])layoutToCopy.Clone();
    }

    public void ResizeWithOffsetData(IntVector2 newSize, IntVector2 offset) =>
        Layout = Layout?.ResizeArrayWithOffsetData(newSize, offset, EMPTY);

    public void CheckToDropItem(int tileId, IntVector2 position)
    {
        var itemType = Tileset.ItemMapper?[tileId];
        if (itemType == null)
            return;
        var absoluteTilePosition = position * Settings.TileSize + Room.PositionInPixels + Settings.TileSize / 2;
        EntityManager.CreateEntityAt(itemType, absoluteTilePosition);
    }

    public void Draw()
    {
        if (Layout == null)
            return;

        // Cached values for performance
        var layout = Layout;
        var texture = Tileset.Texture;
        var tileSize = Settings.TileSize;
        var roomPixelPosition = Room.PositionInPixels;
        var cameraBounds = Camera.DrawScreenLimits;

        // Draw only what is visible on screen
        var startingX = Math.Max(0, (cameraBounds.Left - roomPixelPosition.X) / tileSize.X);
        var startingY = Math.Max(0, (cameraBounds.Top - roomPixelPosition.Y) / tileSize.Y);
        var endingX = Math.Min(Size.Width, (cameraBounds.Right - roomPixelPosition.X + tileSize.X) / tileSize.X);
        var endingY = Math.Min(Size.Height, (cameraBounds.Bottom - roomPixelPosition.Y + tileSize.Y) / tileSize.Y);

        for (var x = startingX; x < endingX; x++)
        {
            for (var y = startingY; y < endingY; y++)
            {
                var type = layout[x, y];
                if (type < 0)
                    continue;

                var sourceRectangle = Tileset.GetSourceRectangle(type);
                var pixelX = roomPixelPosition.X + x * tileSize.X;
                var pixelY = roomPixelPosition.Y + y * tileSize.Y;
                var pixelPosition = new Vector2(pixelX, pixelY);

                Drawer.DrawTextureRectangleAt(texture, sourceRectangle, pixelPosition);
            }
        }
    }
}