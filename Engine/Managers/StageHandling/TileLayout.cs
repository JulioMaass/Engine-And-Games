using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Managers.StageEditing;
using Engine.Types;
using System;

namespace Engine.Managers.StageHandling;

public class TileLayout : Layer // TODO: Rename to layout?
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

        for (var x = 0; x < Size.Width; x++)
        {
            for (var y = 0; y < Size.Height; y++)
            {
                var type = Layout[x, y];
                var tilePosition = IntVector2.New(x, y);
                DrawTile(tilePosition, type);
            }
        }
    }

    private void DrawTile(IntVector2 tilePosition, int type)
    {
        if (type < 0)
            return;

        var sourceRectangle = Drawer.GetSourceRectangleFromId(Tileset.Texture, IntVector2.Zero, Settings.TileSize, type);
        var pixelPosition = Room.PositionInPixels + tilePosition * Settings.TileSize;

        if (!Camera.GetDrawScreenLimits().Overlaps(new IntRectangle(pixelPosition, sourceRectangle.Size)))
            return;
        Drawer.DrawTextureRectangleAt(Tileset.Texture, sourceRectangle, pixelPosition);
    }
}