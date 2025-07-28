using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;
using Engine.Types;
using System.Collections.Generic;
using System.Linq;

namespace MMDB.GameSpecific.Tilesets;

public class VacuumTileset : Tileset
{
    public VacuumTileset()
    {
        Texture = Drawer.TextureDictionary.GetValueOrDefault("VacuumTileset");

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
        DebugMapper[14] = TileType.StairTop;
        DebugMapper[14 + 16] = TileType.Stair;
        DebugMapper[14 + 32] = TileType.SolidTop;
    }
}
