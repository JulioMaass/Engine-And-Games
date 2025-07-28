using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;
using Engine.Types;
using System.Collections.Generic;
using System.Linq;

namespace SpaceMiner.GameSpecific.Tilesets;

public class SpaceMinerTestTileset : Tileset
{
    public SpaceMinerTestTileset()
    {
        Texture = Drawer.TextureDictionary.GetValueOrDefault("TestTileset");

        AddLayerToMapper(LayerId.ForegroundTiles, 4);
        AddLayerToMapper(LayerId.ShadowTiles, 2);
        AddLayerToMapper(LayerId.BackgroundTiles, 4);
        AddLayerToMapper(LayerId.ParalaxTiles, 3);
        AddLayerToMapper(LayerId.LastBackgroundTiles, 3);

        GenerateDebugMapper();
    }

    private void GenerateDebugMapper()
    {
        var totalForegroundTiles = GetTotalForegroundTiles();
        DebugMapper = Enumerable.Repeat(TileType.Solid, totalForegroundTiles).ToList();
        DebugMapper[8] = TileType.BypassSpikes;
        DebugMapper[9] = TileType.BypassSpikes;
        DebugMapper[10] = TileType.BypassSpikes;
        DebugMapper[11] = TileType.BypassSpikes;
        DebugMapper[13] = TileType.StairTop;
    }
}
