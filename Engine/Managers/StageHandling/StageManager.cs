using Engine.ECS.Entities;
using Engine.ECS.Entities.Shared;
using Engine.GameSpecific;
using Engine.Helpers;
using Engine.Main;
using Engine.Managers.Audio;
using Engine.Managers.Graphics;
using Engine.Managers.SaveSystem;
using Engine.Managers.StageEditing;
using Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers.StageHandling;

public static class StageManager // Used to show current room (tiles and entities)
{
    public static string CurrentStageName { get; set; }
    public static Stage CurrentStage { get; private set; }
    public static Dictionary<string, Stage> LoadedStages { get; } = new();
    public static List<Tileset> TilesetList { get; } = new();
    public static Room CurrentRoom { get; set; }
    private static Room TransitioningOutRoom { get; set; } // TODO: Make a transition manager
    public static bool IsTransitioning { get; private set; }
    public static int TransitionFrame { get; private set; }
    public static IntVector2 TransitionFrames { get; } = IntVector2.New(32, 28);
    private static int CurrentTransitionFrames => (TransitionFrames * TransitionDirection).Abs().GetBiggestAxis(); // Get total frames depending on direction
    public static IntVector2 TransitionDirection { get; private set; }
    private static int _topMargin;
    private static int _bottomMargin;

    public static void Initialize()
    {
        // Load tilesets
        foreach (var tilesetType in GameManager.GameSpecificSettings.TilesetTypes)
            TilesetList.Add((Tileset)Activator.CreateInstance(tilesetType));
        StageEditor.TileMode.SetTilesetOfType(GameManager.GameSpecificSettings.DefaultTilesetType);
        // Load stages
        foreach (var stageFileName in GameManager.GameSpecificSettings.StageFiles)
        {
            LoadedStages.Add(stageFileName, JsonHandler.LoadStageFromFile(stageFileName));
        }
    }

    public static void GameSpecificSetup(int topMargin, int bottomMargin)
    {
        _topMargin = topMargin;
        _bottomMargin = bottomMargin;
    }

    public static void RestartStage()
    {
        CurrentStage.RespawnPosition = IntVector2.Zero;
        RespawnPlayer();
    }

    public static void LoadStage()
    {
        CurrentStage = LoadedStages[CurrentStageName];
    }

    public static Stage GenerateEmptyStage()
    {
        var stageSize = IntVector2.New(3, 3);
        var stage = new Stage(stageSize);
        CurrentStage = stage;
        stage.AddNewRoom(IntVector2.New(1, 1));
        return stage;
    }

    public static void CheckToTriggerSpawns() // TODO: Make a spawn manager
    {
        if (CurrentRoom == null)
            return;

        foreach (var entityInstance in CurrentRoom.GetEntityLayout().List)
        {
            if (CollectionManager.GetEntityFromType(entityInstance.EntityType).SpawnManager.AutomaticSpawn == false)
                continue;

            CheckToResetSpawner(entityInstance);
            CheckToSpawnEntity(entityInstance);
        }
    }

    private static void CheckToResetSpawner(EntityInstance entityInstance)
    {
        if (entityInstance.SpawnedEntity != null || entityInstance.CanSpawn)
            return;
        DebugMode.SpawnCollisionCounter++;
        if (!Camera.GetSpawnScreenLimitsWithBorder(Settings.TileSize).Overlaps(entityInstance.GetCollisionBox()))
            entityInstance.CanSpawn = true;
    }

    private static void CheckToSpawnEntity(EntityInstance entityInstance)
    {
        if (!entityInstance.CanSpawn || entityInstance.SpawnedEntity != null)
            return;
        DebugMode.SpawnCollisionCounter++;
        if (Camera.GetSpawnScreenLimits().Overlaps(entityInstance.GetCollisionBox())
            || CollectionManager.GetEntityFromType(entityInstance.EntityType).SpawnManager.EarlySpawn)
            EntityManager.CreateEntityInstance(entityInstance);
    }

    public static Room GetStartingRoom()
    {
        var startingRoom = CurrentStage.GetRoomAtPixel(CurrentStage.PlayerStartingPosition);
        return startingRoom ?? CurrentStage.RoomList[0];
    }

    public static void CheckToRespawnPlayer()
    {
        if (EntityManager.PlayerEntity != null)
            return;
        if (!PlayerSpawnManager.DeathTimeReached())
            return;
        RespawnPlayer();
    }

    private static void RespawnPlayer()
    {
        AudioManager.PlayMusic("musMM6BossFull"); // TODO - TEMPORARY: Change music depending on stage

        // Fade in screen and text
        ScreenDimmer.DimScreen(0f, 1f, 20, 5);
        if (GameManager.GameSpecificSettings.CurrentGame == GameId.Mmdb)
            ScreenTextManager.CreateTemporaryText("READY", 120, 30, 6, 6);

        // Delete all entities
        CurrentRoom?.ResetAllSpawners();
        foreach (var entity in EntityManager.GetAllEntities())
            EntityManager.DeleteEntity(entity);

        // Set room and player position
        var spawningPosition = GetPlayerRespawnPosition();
        if (spawningPosition == IntVector2.Zero)
            spawningPosition = CurrentStage.RoomList.FirstOrDefault()!.PositionInPixels + Settings.ScreenSize / 2;
        CurrentRoom = CurrentStage.GetRoomAtPixel(spawningPosition) ?? GetStartingRoom();

        // Create entities
        CurrentRoom?.ResetAllSpawners();
        EntityManager.CreateEntityAt(GameManager.GameSpecificSettings.PlayerTypes.FirstOrDefault(), spawningPosition);
        Camera.FocusOnPlayer();
        RespawnAllPermanents();
    }

    private static void RespawnAllPermanents()
    {
        foreach (var room in CurrentStage.RoomList)
            foreach (var entityInstance in room.GetEntityLayout().List)
                if (CollectionManager.GetEntityFromType(entityInstance.EntityType).SpawnManager.Permanent)
                    EntityManager.CreateEntityInstance(entityInstance);
    }

    private static IntVector2 GetPlayerRespawnPosition()
    {
        return CurrentStage.RespawnPosition == IntVector2.Zero ? CurrentStage.PlayerStartingPosition : CurrentStage.RespawnPosition;
    }

    public static void UpdatePlayerRoom() // Check for transitions
    {
        if (EntityManager.PlayerEntity == null)
            return;

        var transitionRooms = GetTransitionRooms();
        if (transitionRooms.Count == 0)
            return;

        (Room Room, IntVector2 Direction) roomAndDirection = GetRoomsInEachDirection()
            .FirstOrDefault(roomAndDirection => transitionRooms.Contains(roomAndDirection.room));

        if (CantTransitionUp(roomAndDirection))
            return;
        if (CantTransitionToSide(roomAndDirection))
            return;

        PushPlayerInBounds();
        TransitioningOutRoom = CurrentRoom;
        CurrentRoom = roomAndDirection.Room;
        IsTransitioning = true;
        TransitionFrame = 0;
        Camera.SaveTransitionReferencePoints();
        TransitionDirection = roomAndDirection.Direction;
        TransitioningOutRoom.GetEntityLayout().List.ForEach(entity => entity.CanSpawn = true);
        UpdateRespawningPoint();

        CheckToRunTransition();
    }

    private static List<Room> GetTransitionRooms()
    {
        var collisionBox = EntityManager.PlayerEntity.CollisionBox;
        var transitionCheckingPoints = new List<IntVector2>
        {
            IntVector2.New(collisionBox.GetCollisionRectangle().Left, Camera.CameraCenter.Y), // left
            IntVector2.New(collisionBox.GetCollisionRectangle().Right, Camera.CameraCenter.Y), // right
            IntVector2.New(Camera.CameraCenter.X, collisionBox.GetCollisionRectangle().Top), // top
            IntVector2.New(Camera.CameraCenter.X, collisionBox.GetCollisionRectangle().Bottom) // bottom
        };

        var collidingRooms = transitionCheckingPoints.Select(side => CurrentStage.GetRoomAtPixel(side))
            .Where(room => room != null).Distinct().ToList();
        var currentRoom = CurrentStage.GetRoomAtPixel(Camera.CameraCenter);

        return collidingRooms.Without(currentRoom).ToList();
    }

    private static List<(Room room, IntVector2 direction)> GetRoomsInEachDirection()
    {
        var directions = new List<IntVector2> { IntVector2.PixelRight, IntVector2.PixelLeft, IntVector2.PixelUp, IntVector2.PixelDown };
        var roomAndDirectionList = new List<(Room room, IntVector2 direction)>();
        foreach (var direction in directions)
        {
            var room = CurrentStage.GetRoomAtPixel(Camera.CameraCenter + direction * Settings.RoomSizeInPixels);
            roomAndDirectionList.Add((room, direction));
        }
        return roomAndDirectionList;
    }

    private static bool CantTransitionUp((Room Room, IntVector2 Direction) roomAndDirection)
    {
        if (EntityManager.PlayerEntity.TransitionController == null)
            return false;
        return !EntityManager.PlayerEntity.TransitionController.UpCondition.AllConditionsAreTrue()
               && roomAndDirection.Direction == IntVector2.PixelUp;
    }

    private static bool CantTransitionToSide((Room Room, IntVector2 Direction) roomAndDirection)
    {
        if (EntityManager.PlayerEntity.TransitionController == null)
            return false;
        return !EntityManager.PlayerEntity.TransitionController.SidesCondition.AllConditionsAreTrue()
               && (roomAndDirection.Direction == IntVector2.PixelLeft
               || roomAndDirection.Direction == IntVector2.PixelRight);
    }

    private static void UpdateRespawningPoint()
    {
        foreach (var entityInstance in CurrentRoom.GetEntityLayout().List
            .Where(entityInstance => entityInstance.EntityType == typeof(RespawnPoint)))
            CurrentStage.RespawnPosition = entityInstance.PositionAbsolute;
    }

    public static void CheckToRunTransition()
    {
        if (!IsTransitioning) return;

        // TODO: On transition start, delete entities outside of screen

        // End transition
        if (TransitionFrame >= CurrentTransitionFrames)
        {
            ClearEntitiesFromOtherRooms();
            TransitioningOutRoom = null;
            IsTransitioning = false;
            TransitionFrame = 0;
            return;
        }

        // Run transition
        TransitionFrame++;
        EntityManager.PlayerEntity.Position.Pixel += TransitionDirection;

        // Update some player properties
        if (EntityManager.PlayerEntity.StateManager.CurrentState.UpdatesFrameOnTransitions)
        {
            EntityManager.PlayerEntity.StateManager.CurrentState.Frame++;
            EntityManager.PlayerEntity.StateManager.CurrentState.AnimationFrame++;
        }
        if (EntityManager.PlayerEntity.ChargeManager?.GetChargeTier() > 0)
            EntityManager.PlayerEntity.ChargeManager?.Update();
    }

    public static void PushPlayerInBounds()
    {
        if (EntityManager.PlayerEntity == null) return;
        if (IsTransitioning && TransitionFrame != 0) return;

        var collisionBox = EntityManager.PlayerEntity.CollisionBox.GetCollisionRectangle();
        var collisionRoomPosition = CurrentRoom.PositionInPixels - IntVector2.New(0, _topMargin);
        var collisionRoomSize = CurrentRoom.SizeInPixels + IntVector2.New(0, _topMargin + _bottomMargin);
        var roomCollisionBox = new IntRectangle(collisionRoomPosition, collisionRoomSize);
        var movement = collisionBox.DistanceToGetInside(roomCollisionBox);

        EntityManager.PlayerEntity.Position.Pixel += movement;
    }

    private static void ClearEntitiesFromOtherRooms()
    {
        foreach (var entity in EntityManager.GetAllEntities()
            .Where(entity => entity.SpawnManager.Room != CurrentRoom)
            .Where(entity => !entity.SpawnManager.PersistsOnTransitions)
            .Where(entity => !entity.SpawnManager.Permanent))
            EntityManager.DeleteEntity(entity);
    }

    public static List<Room> GetRoomsToDraw()
    {
        return StageEditor.IsOn
            ? CurrentStage.RoomList
            : [CurrentRoom, TransitioningOutRoom];
    }
}
