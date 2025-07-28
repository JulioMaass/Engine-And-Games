using Engine.Main;
using Engine.Managers.StageHandling;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Engine.Types;

public abstract class Tileset
{
    public Texture2D Texture { get; protected init; }
    public List<(LayerId id, int lines)> LayerMapper { get; } = new(); // Used to determine which layer a tile should be drawn on (based on tile menu Y position)
    public List<TileType> DebugMapper { get; set; }
    public List<Type> ItemMapper { get; set; }

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
