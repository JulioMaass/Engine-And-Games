using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;
using Engine.Types;
using Mole.GameSpecific.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mole.GameSpecific.Tilesets;

public class MoleTileset : Tileset
{
    public MoleTileset()
    {
        Texture = Drawer.TextureDictionary.GetValueOrDefault("MoleTileset");

        AddLayerToMapper(LayerId.ForegroundTiles, 7);
        AddLayerToMapper(LayerId.BackgroundTiles, 5);

        GenerateDebugMapper();
        GenerateItemMapper();
    }

    private void GenerateDebugMapper() // TODO: Make function for this on Tileset (parent)
    {
        var totalForegroundTiles = GetTotalForegroundTiles();
        DebugMapper = Enumerable.Repeat(TileType.Solid, totalForegroundTiles).ToList();
        for (var i = 0; i < totalForegroundTiles; i++)
        {
            if (i < 32)
                DebugMapper[i] = TileType.DestructibleWeak;
            else if (i == 32)
                DebugMapper[i] = TileType.DestructibleStrong;
        }
    }

    private void GenerateItemMapper() // TODO: Make function for this on Tileset (parent)
    {
        var totalForegroundTiles = GetTotalForegroundTiles();
        ItemMapper = Enumerable.Repeat<Type>(null, totalForegroundTiles).ToList();
        for (var i = 0; i < totalForegroundTiles; i++)
        {
            if (i == 8)
                ItemMapper[i] = typeof(GoldItem);
            else if (i == 16)
                ItemMapper[i] = typeof(MoleBomb);
            else if (i == 24)
                ItemMapper[i] = typeof(BombItem);
        }
    }

}
