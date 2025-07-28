using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;
using Engine.Types;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers.Ai;

public static class AiManager
{
    public static int[,] DistanceMap { get; private set; }
    private const int FREE_TILE = 0;
    private const int OCCUPIED_TILE = 999;

    private static void CreateDistanceMap()
    {
        DistanceMap = Extensions.NewArray(StageManager.CurrentRoom.SizeInTiles, OCCUPIED_TILE);
    }

    public static void UpdateDistanceMap() // TODO: Organize code and put it in functions
    {
        CreateDistanceMap();

        var playerTile = EntityManager.PlayerEntity.Position.Pixel.RoundDownToTileCoordinate() - StageManager.CurrentRoom.PositionInTiles;
        var tilesToCheck = new List<IntVector2> { playerTile };
        DistanceMap[playerTile.X, playerTile.Y] = 0; // TODO: Game crashes if mole leaves the room (north or south)

        // Spread distance map from player
        while (tilesToCheck.Count > 0)
        {
            foreach (var tile in tilesToCheck.ToList())
            {
                var neighbors = tile.GetNeighbors();
                foreach (var neighbor in neighbors
                    .Where(neighbor => StageManager.CurrentRoom.DebugTiles.GetTileTypeAt(neighbor) == TileType.NoTile
                                       && DistanceMap[neighbor.X, neighbor.Y] > DistanceMap[tile.X, tile.Y] + 1))
                {
                    DistanceMap[neighbor.X, neighbor.Y] = DistanceMap[tile.X, tile.Y] + 1;
                    tilesToCheck.Add(neighbor);
                }
                tilesToCheck.Remove(tile);
            }
        }
    }

    public static void DrawDistanceMap()
    {
        if (DistanceMap == null) return;

        for (var x = 0; x < DistanceMap.GetLength(0); x++)
        {
            for (var y = 0; y < DistanceMap.GetLength(1); y++)
            {
                if (DistanceMap[x, y] >= OCCUPIED_TILE) continue;

                var position = IntVector2.New(x * 16, y * 16) + StageManager.CurrentRoom.PositionInPixels;
                var text = DistanceMap[x, y].ToString();
                Video.SpriteBatch.DrawString(Drawer.HpFontMap, text, position, CustomColor.White);
            }
        }
    }
}
