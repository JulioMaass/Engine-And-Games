using Engine.Helpers;
using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using Color = Microsoft.Xna.Framework.Color;

namespace Engine.Managers.StageHandling;

public class DebugTiles
{
    private Room Room { get; }

    public DebugTiles(IntVector2 size, Room room)
    {
        Room = room;
    }

    public bool IsThereTileWithPropertyInRectangle(IntRectangle absoluteTileRectangle, TileProperty tileProperty)
    {
        var relativeTileRectangle = absoluteTileRectangle.OffsetPosition(-Room.PositionInTiles);
        for (var x = relativeTileRectangle.Left; x <= relativeTileRectangle.Right; x++)
            for (var y = relativeTileRectangle.Top; y <= relativeTileRectangle.Bottom; y++)
                if (GetTilePropertiesInBoundsValueAt(x, y).Contains(tileProperty))
                    return true;
        return false;
    }

    public List<TileProperty> GetTilePropertiesInBoundsValueAt(int x, int y)
    {
        var tileType = GetTileTypeInBoundsValueAt(IntVector2.New(x, y));
        return GetTileTypeProperties(tileType);
    }

    private static TileType GetDebugTypeFromTileId(int tileId, Tileset tileset) =>
        tileset.DebugMapper.GetValueAtOrDefault(tileId, TileType.NoTile);

    public TileType GetTileTypeAt(IntVector2 position)
    {
        if (!IsInBounds(position))
            return TileType.Default;

        foreach (var tileLayout in Room.Layers.OfType<TileLayout>()
                     .Where(tileLayout => tileLayout.LayerId == LayerId.ForegroundTiles)
                     .Where(tileLayout => tileLayout.Layout != null).ToList())
        {
            var tileId = tileLayout.GetValueAt(position);
            var debugValue = GetDebugTypeFromTileId(tileId, tileLayout.Tileset);
            if (debugValue == TileType.NoTile)
                continue;
            return debugValue;
        }
        return TileType.NoTile;
    }

    private bool IsInBounds(IntVector2 position)
    {
        return Room.Layers.OfType<TileLayout>()
            .Where(tileLayout => tileLayout.LayerId == LayerId.ForegroundTiles)
            .Where(tileLayout => tileLayout.Layout != null)
            .Any(tileLayout => tileLayout.Layout.IsInBounds(position));
    }

    public List<TileProperty> GetTilePropertiesAt(IntVector2 position)
    {
        var tileType = GetTileTypeAt(position);
        return GetTileTypeProperties(tileType);
    }

    public TileType GetTileTypeInBoundsValueAt(IntVector2 position)
    {
        // If edge tile is solid, out of bounds returns solid
        var inBoundsPosition = IntVector2.New(
            Math.Clamp(position.X, 0, Room.SizeInTiles.X - 1),
            Math.Clamp(position.Y, 0, Room.SizeInTiles.Y - 1)
        );
        if (inBoundsPosition != position
            && GetTileTypeAt(inBoundsPosition) == TileType.Solid)
            return TileType.Solid;

        // Else return the tile value (NO_TILE when out of bounds)
        return GetTileTypeAt(position);
    }

    public void Draw()
    {
        var roomSize = Room.SizeInTiles;
        for (var x = 0; x < roomSize.X; x++)
            for (var y = 0; y < roomSize.Y; y++)
            {
                var tilePosition = IntVector2.New(x, y);
                var debugType = GetTileTypeAt(tilePosition);
                DrawTile(tilePosition, debugType);
            }
    }

    private void DrawTile(IntVector2 tilePosition, TileType type)
    {
        var pixelPosition = Room.PositionInPixels + tilePosition * Settings.TileSize;
        if (!Camera.GetDrawScreenLimits().Overlaps(new IntRectangle(pixelPosition, Settings.TileSize)))
            return;
        var color = GetTypeColor(type);
        Drawer.DrawRectangle(pixelPosition, Settings.TileSize, color);
    }

    public static Color GetTypeColor(TileType type)
    {
        return type switch
        {
            TileType.Solid => CustomColor.TransparentWhite,
            TileType.SolidTop => CustomColor.TransparentBlue,
            TileType.Stair => CustomColor.TransparentGreen,
            TileType.StairTop => CustomColor.TransparentCyan,
            TileType.SolidSpikes => CustomColor.TransparentRed,
            TileType.BypassSpikes => CustomColor.TransparentRed,
            TileType.DestructibleWeak => CustomColor.TransparentGreen,
            TileType.DestructibleStrong => CustomColor.TransparentYellow,
            _ => CustomColor.Transparent
        };
    }
    public static List<TileProperty> GetTileTypeProperties(TileType tileType)
    {
        var properties = new List<TileProperty>();
        if (tileType == TileType.Solid)
        {
            properties.Add(TileProperty.Solid);
        }
        else if (tileType == TileType.SolidTop)
        {
            properties.Add(TileProperty.SolidTop);
        }
        else if (tileType == TileType.Stair)
        {
            properties.Add(TileProperty.Stair);
        }
        else if (tileType == TileType.StairTop)
        {
            properties.Add(TileProperty.SolidTop);
            properties.Add(TileProperty.Stair);
        }
        else if (tileType == TileType.BypassSpikes)
        {
            properties.Add(TileProperty.Spikes);
        }
        else if (tileType == TileType.SolidSpikes)
        {
            properties.Add(TileProperty.Solid);
            properties.Add(TileProperty.Spikes);
        }
        else if (tileType == TileType.DestructibleWeak)
        {
            properties.Add(TileProperty.Solid);
            properties.Add(TileProperty.DestructibleWeak);
            properties.Add(TileProperty.DestructibleStrong);
        }
        else if (tileType == TileType.DestructibleStrong)
        {
            properties.Add(TileProperty.Solid);
            properties.Add(TileProperty.DestructibleStrong);
        }
        return properties;
    }
}

public enum TileType
{
    NoTile = -1,
    Default = 0,
    Solid = 1,
    SolidTop = 2,
    Stair = 3,
    StairTop = 4,
    SolidSpikes = 5,
    BypassSpikes = 6,
    DestructibleWeak = 7,
    DestructibleStrong = 8,
}

public enum TileProperty // Used to check a property only, instead of the whole tile type (Ex: Destructible is solid and destructible)
{
    None,
    Solid,
    SolidTop,
    Stair,
    Spikes,
    DestructibleWeak,
    DestructibleStrong,
}