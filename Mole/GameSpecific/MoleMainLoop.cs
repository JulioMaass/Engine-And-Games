using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Main;
using Engine.Managers.Ai;
using Engine.Managers.CollisionSystem;
using Engine.Managers.GameModes;
using Engine.Managers.GlobalManagement;
using Engine.Managers.StageEditing;
using Engine.Managers.StageHandling;
using Engine.Types;
using Mole.GameSpecific.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable MethodTooLong

namespace Mole.GameSpecific;

public class MoleMainLoop : GameLoop
{
    private Engine.ECS.Entities.EntityCreation.Entity PlayerEntity => EntityManager.PlayerEntity;
    private int Level { get; set; } = 1; // TODO: TEMPORARY: Move to Global Values if this is going to be kept

    public override void GameSpecificSetup() // TODO: Reenable MethodTooLong and improve this file
    {
        // Save player position
        StageManager.CurrentStage.RespawnPosition = PlayerEntity.Position.Pixel;

        // Starting resources
        GlobalManager.Values.MainCharData.Resources.AddNew(ResourceType.Bombs, 9, 3);
        GlobalManager.Values.MainCharData.Resources.AddNew(ResourceType.Gold, 99, 0);

        // Remove all rooms
        foreach (var room in StageManager.CurrentStage.RoomList.ToList())
            StageManager.CurrentStage.RemoveRoomAt(room.Position);

        // Create new rooms
        for (var x = 0; x < 3; x++)
            for (var y = 0; y < 3; y++)
                StageManager.CurrentStage.AddNewRoom(new IntVector2(x, y));

        // Recreate player
        StageManager.CheckToRespawnPlayer();

        // Choose rooms for stairs
        var roomsWithStairs = StageManager.CurrentStage.RoomList.ToList();
        roomsWithStairs.Remove(StageManager.CurrentRoom);
        roomsWithStairs.Shuffle();
        var stairsRooms = roomsWithStairs.GetRange(0, 3);

        // Randomize room layout
        foreach (var room in StageManager.CurrentStage.RoomList.ToList())
        {
            var hasStairs = stairsRooms.Contains(room);
            RandomizeRoomLayout(room, hasStairs);
        }

        MakeRoomPaths();
        MakeRoomPaths(2);

        // Place stair on starting position and clear foreground
        if (PlayerEntity != null) // TODO: TEMPORARY: Should make sure player is always created instead
        {
            var playerTilePosition = PlayerEntity.Position.Pixel.RoundDownToTileCoordinate();
            PlayerEntity.Position.Pixel = playerTilePosition * Settings.TileSize + Settings.TileSize / 2;
            StageManager.CurrentRoom.GetTileLayout(LayerId.ForegroundTiles, StageEditor.TileMode.CurrentTileset.GetType()).SetTileAt(playerTilePosition - StageManager.CurrentRoom.PositionInTiles, 80);
        }
    }

    private void MakeRoomPaths(int totalPaths = 0)
    {
        if (totalPaths == 0)
            totalPaths = StageManager.CurrentStage.RoomList.Count - 1;
        var initialRoom = StageManager.CurrentStage.RoomList.GetRandom();
        var linkedRoomList = new List<Room> { initialRoom };

        while (linkedRoomList.Count <= totalPaths)
        {
            // Get room and neighbor
            var linkedRoom = linkedRoomList.GetRandom();
            var linkedRoomPosition = linkedRoom.Position;
            var neighborPositions = linkedRoomPosition.GetNeighbors();
            foreach (var neighborPosition in neighborPositions.ToList())
                if (!StageManager.CurrentStage.HasRoomAt(neighborPosition))
                    neighborPositions.Remove(neighborPosition);
            var neighborRandomPosition = neighborPositions.GetRandom();
            var neighborRoom = StageManager.CurrentStage.GetRoomAtGrid(neighborRandomPosition);

            // Check if room is already linked
            if (linkedRoomList.Contains(neighborRoom))
                continue;
            linkedRoomList.Add(neighborRoom);

            // Get tile layout and clear path
            var xTileOffset = GetRandom.UnseededInt(2, 18);
            var yTileOffset = GetRandom.UnseededInt(2, 9);
            var linkedRoomTileLayout = linkedRoom.GetTileLayout(LayerId.ForegroundTiles, StageEditor.TileMode.CurrentTileset.GetType());
            var neighborRoomTileLayout = neighborRoom.GetTileLayout(LayerId.ForegroundTiles, StageEditor.TileMode.CurrentTileset.GetType());
            var positionDifference = neighborRandomPosition - linkedRoomPosition;
            if (positionDifference.X != 0)
            {
                var x1 = positionDifference.X == -1 ? 0 : linkedRoomTileLayout.Size.Width - 1;
                var x2 = positionDifference.X == -1 ? 1 : linkedRoomTileLayout.Size.Width - 2;
                var y = yTileOffset;
                linkedRoomTileLayout.SetTileAt((x1, y), TileLayout.EMPTY);
                linkedRoomTileLayout.SetTileAt((x2, y), TileLayout.EMPTY);
            }
            if (positionDifference.Y != 0)
            {
                var x = xTileOffset;
                var y1 = positionDifference.Y == -1 ? 0 : linkedRoomTileLayout.Size.Height - 1;
                var y2 = positionDifference.Y == -1 ? 1 : linkedRoomTileLayout.Size.Height - 2;
                linkedRoomTileLayout.SetTileAt((x, y1), TileLayout.EMPTY);
                linkedRoomTileLayout.SetTileAt((x, y2), TileLayout.EMPTY);
            }
            var positionDifferenceNeighbor = linkedRoomPosition - neighborRandomPosition;
            if (positionDifferenceNeighbor.X != 0)
            {
                var x1 = positionDifferenceNeighbor.X == -1 ? 0 : neighborRoomTileLayout.Size.Width - 1;
                var x2 = positionDifferenceNeighbor.X == -1 ? 1 : neighborRoomTileLayout.Size.Width - 2;
                var y = yTileOffset;
                neighborRoomTileLayout.SetTileAt((x1, y), TileLayout.EMPTY);
                neighborRoomTileLayout.SetTileAt((x2, y), TileLayout.EMPTY);
            }
            if (positionDifferenceNeighbor.Y != 0)
            {
                var x = xTileOffset;
                var y1 = positionDifferenceNeighbor.Y == -1 ? 0 : neighborRoomTileLayout.Size.Height - 1;
                var y2 = positionDifferenceNeighbor.Y == -1 ? 1 : neighborRoomTileLayout.Size.Height - 2;
                neighborRoomTileLayout.SetTileAt((x, y1), TileLayout.EMPTY);
                neighborRoomTileLayout.SetTileAt((x, y2), TileLayout.EMPTY);
            }
        }
    }

    public override void Update() // TODO: ARCHITECTURE: Make functions for each part of the update once it has better code (placeholder for now). Move to relevant components if possible
    {
        // Update duration counters
        AdvanceDurationCounter();
        foreach (var entity in EntityManager.GetAllEntities())
            entity.StateManager?.AddFrame();

        // Collision handling (it happens 1 frame later, so the player can see the hit before the outcome)
        CollisionHandler.EntityTypeGetItems(EntityKind.Player);

        // Player controls and state
        PlayerEntity?.PlayerControl.Update();
        PlayerEntity?.StateManager.Update();

        // Player physics
        PlayerEntity?.Physics.SolidCollidingMovement.MoveToSolidY();
        PlayerEntity?.Physics.SolidCollidingMovement.MoveToSolidX();

        AiManager.UpdateDistanceMap();

        // Enemy physics
        foreach (var enemy in EntityManager.GetFilteredEntitiesFrom(EntityKind.Enemy).ToList())
            enemy.Update();

        // Shot physics
        foreach (var shot in EntityManager.GetFilteredEntitiesFrom(EntityKind.EnemyShot).ToList())
            shot.Update();

        // Create slash in front of the player (based on facing)
        if (PlayerEntity?.PlayerControl.Button1Press == true)
        {
            var slashPosition = PlayerEntity.Position.Pixel + IntVector2.New(PlayerEntity.Facing.X, PlayerEntity.Facing.Y) * Settings.TileSize;
            EntityManager.CreateEntityAt<MoleSlash>(slashPosition);
        }
        if (PlayerEntity?.PlayerControl.Button3Press == true)
        {
            var drillPosition = PlayerEntity.Position.Pixel + IntVector2.New(PlayerEntity.Facing.X, PlayerEntity.Facing.Y) * Settings.TileSize / 2;
            var drillShot = EntityManager.CreateEntityAt<BazookaShot>(drillPosition);
            drillShot.Facing.CopyFacingFrom(PlayerEntity);
        }
        if (PlayerEntity?.PlayerControl.Button4Press == true && GlobalManager.Values.MainCharData.Resources.HasResource(ResourceType.Bombs, 1))
        {
            GlobalManager.Values.MainCharData.Resources.AddAmount(ResourceType.Bombs, -1);
            var bombPosition = PlayerEntity.Position.Pixel;
            EntityManager.CreateEntityAt<MoleBomb>(bombPosition);
        }
        foreach (var shot in EntityManager.GetFilteredEntitiesFrom(EntityKind.PlayerShot).ToList())
            shot.Update();

        foreach (var entity in EntityManager.GetAllEntities())
            entity.TileDestructor?.Update();

        // Destroy enemy if colliding with slash
        foreach (var enemy in EntityManager.GetFilteredEntitiesFrom(EntityKind.Enemy).ToList())
        {
            foreach (var slash in EntityManager.GetFilteredEntitiesFrom(EntityKind.PlayerShot).ToList())
            {
                if (enemy.CollisionBox?.CollidesWithEntityPixel(slash) != true) continue;
                if (slash.FrameHandler.CurrentFrame != 0) continue;
                EntityManager.DeleteEntity(enemy);
            }
        }

        // Go to next stage
        var playerTilePosition = PlayerEntity!.Position.Pixel.RoundDownToTileCoordinate() - StageManager.CurrentRoom.PositionInTiles;
        var backgroundTileLayout = StageManager.CurrentRoom.GetTileLayout(LayerId.BackgroundTiles, StageEditor.TileMode.CurrentTileset.GetType());
        if (backgroundTileLayout.GetValueAt(playerTilePosition) == 88)
        {
            Level += 2;
            GameSpecificSetup();
        }
    }

    private void AdvanceDurationCounter() // TODO: Include this in parent class
    {
        foreach (var entity in EntityManager.GetAllEntities())
        {
            entity.FrameHandler?.AdvanceFrameCounter();
        }
    }

    private void RandomizeRoomLayout(Room room, bool addStairs)
    {
        // Make floor and walls
        var foregroundTileLayout = room.GetTileLayout(LayerId.ForegroundTiles, StageEditor.TileMode.CurrentTileset.GetType());
        var backgroundTileLayout = room.GetTileLayout(LayerId.BackgroundTiles, StageEditor.TileMode.CurrentTileset.GetType());
        for (var x = 0; x < foregroundTileLayout.Size.Width; x++)
        {
            for (var y = 0; y < foregroundTileLayout.Size.Height; y++)
            {
                // Clear background layout
                var position = IntVector2.New(x, y);
                backgroundTileLayout.SetTileAt(position, 56);
                foregroundTileLayout.SetTileAt(position, -1);

                // Set to hard on borders
                var isTopBorder = y == 0;
                var isBottomBorder = y == foregroundTileLayout.Size.Height - 1;
                var isLeftBorder = x == 0;
                var isRightBorder = x == foregroundTileLayout.Size.Width - 1;
                if (isTopBorder || isBottomBorder || isLeftBorder || isRightBorder)
                    foregroundTileLayout.SetTileAt(position, 33);
            }
        }

        // Make list of all tiles
        var tileList = new List<IntVector2>();
        for (var x = 1; x < foregroundTileLayout.Size.Width - 1; x++)
        {
            for (var y = 1; y < foregroundTileLayout.Size.Height - 1; y++)
            {
                tileList.Add(IntVector2.New(x, y));
            }
        }

        // Place stair on random position
        if (addStairs)
        {
            var randomTile = tileList.GetRandom();
            backgroundTileLayout.SetTileAt(randomTile, 88);
            tileList.Remove(randomTile);
        }

        // Place enemy on random position
        var enemyDifficultyPerLevel = Level + 2 + GetRandom.UnseededInt(0, 2);
        var enemiesData = new List<(Type Type, int Difficulty)> // Enemy type and difficulty
        {
            (typeof(EnemyWalker), 1),
            (typeof(SpiderBot), 2),
            (typeof(TurretBot), 3),
        };
        while (enemyDifficultyPerLevel > 0)
        {
            var randomTile = tileList.GetRandom();
            var position = randomTile * Settings.TileSize + Settings.TileSize / 2;
            var enemyData = enemiesData.GetRandom();
            var entityLayout = room.GetEntityLayout();
            var enemyInstance = new EntityInstance(entityLayout, enemyData.Type, position);
            entityLayout.List.Add(enemyInstance);
            tileList.Remove(randomTile);
            enemyDifficultyPerLevel -= enemyData.Difficulty;
        }

        // Randomize foreground layout
        var blockPercentage = 70;
        var unbreakablePercentage = 50;
        var blocksToPlace = tileList.Count * blockPercentage / 100;
        for (var i = 0; i < blocksToPlace; i++)
        {
            var randomTile = tileList.GetRandom();
            if (i < blocksToPlace * unbreakablePercentage / 100)
                foregroundTileLayout.SetTileAt(randomTile, 32);
            else
            {
                var chance = GetRandom.UnseededInt(100);
                if (chance < 2)
                    foregroundTileLayout.SetTileAt(randomTile, 24);
                else if (chance < 15)
                    foregroundTileLayout.SetTileAt(randomTile, 16);
                else if (chance < 40)
                    foregroundTileLayout.SetTileAt(randomTile, 8);
                else
                    foregroundTileLayout.SetTileAt(randomTile, 0);
            }
            tileList.Remove(randomTile);
        }
    }
}