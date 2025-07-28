using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;
using Engine.Types;
using System.Collections.Generic;
using System.Linq;

namespace MMDB.GameSpecific.Tilesets;

public class ChainTileset : Tileset
{
    public ChainTileset()
    {
        Texture = Drawer.TextureDictionary.GetValueOrDefault("ChainTileset");

        AddLayerToMapper(LayerId.ForegroundTiles, 7);
        AddLayerToMapper(LayerId.ShadowTiles, 2);
        AddLayerToMapper(LayerId.BackgroundTiles, 4);
        AddLayerToMapper(LayerId.ParalaxTiles, 0);
        AddLayerToMapper(LayerId.LastBackgroundTiles, 3);

        GenerateDebugMapper();
    }

    private void GenerateDebugMapper()
    {
        var totalForegroundTiles = GetTotalForegroundTiles();
        DebugMapper = Enumerable.Repeat(TileType.Solid, totalForegroundTiles).ToList();
        DebugMapper[14] = TileType.StairTop;
        DebugMapper[14 + 16] = TileType.Stair;
        DebugMapper[14 + 32] = TileType.SolidTop;
    }
}
