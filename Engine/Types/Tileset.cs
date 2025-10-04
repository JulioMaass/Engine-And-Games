using Engine.Main;
using Engine.Managers.StageHandling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Engine.Types;

public abstract class Tileset
{
    public Texture2D Texture { get; protected init; }
    private Rectangle[] SourceRectangles { get; set; } // Each tile's source rectangle in the tileset texture, by tile type/id
    public List<(LayerId id, int lines)> LayerMapper { get; } = new(); // Used to determine which layer a tile should be drawn on (based on tile menu Y position)
    public List<TileType> DebugMapper { get; set; }
    public List<Type> ItemMapper { get; set; }

    public Rectangle GetSourceRectangle(int tileType)
    {
        if (SourceRectangles == null)
            InitializeSourceRectangles();
        return SourceRectangles![tileType];
    }

    public void InitializeSourceRectangles()
    {
        var width = Texture.Width / Settings.TileSize.X;
        var height = Texture.Height / Settings.TileSize.Y;
        var tileWidth = Settings.TileSize.X;
        var tileHeight = Settings.TileSize.Y;
        SourceRectangles = new Rectangle[width * height];
        for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                SourceRectangles[y * width + x] = new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight);
    }

    protected void AddLayerToMapper(LayerId layerId, int lines) =>
        LayerMapper.Add((layerId, lines));

    public int GetTotalForegroundLines() =>
        LayerMapper.FirstOrDefault(x => x.id == LayerId.ForegroundTiles).lines;

    protected int GetTotalForegroundTiles() =>
        GetTotalForegroundLines() * Texture.Width / Settings.TileSize.X;

    public LayerId GetIdFromLine(int line)
    {
        foreach (var layer in LayerMapper)
        {
            if (line < layer.lines)
                return layer.id;
            line -= layer.lines;
        }
        Debugger.Break();
        return default;
    }
}
