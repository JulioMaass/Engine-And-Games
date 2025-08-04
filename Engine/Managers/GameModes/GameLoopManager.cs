using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Main;
using Engine.Managers.GlobalManagement;
using Engine.Managers.Graphics;
using Engine.Managers.StageEditing;
using Engine.Managers.StageHandling;
using System;
using System.Diagnostics;
using System.Linq;

namespace Engine.Managers.GameModes;

public static class GameLoopManager
{
    public static GameLoop GameMainLoop { get; private set; }
    public static GameLoop GameCurrentLoop { get; private set; }

    public static void ResetGameLoop()
    {
        StageManager.LoadStage();
        StageManager.RestartStage();

        GameMainLoop = (GameLoop)Activator.CreateInstance(GameManager.GameSpecificSettings.MainLoop);
        SetGameLoop(GameMainLoop);
    }

    public static void SetGameLoop(GameLoop gameLoop)
    {
        GameCurrentLoop = gameLoop;
        GameCurrentLoop.GameSpecificSetup();
    }

    public static void QuitGameLoop()
    {
        GameCurrentLoop = null;
    }

    public static void Update()
    {
        // STAGE MANAGEMENT
        // Transitions and stage update
        StageManager.CheckToRunTransition();
        if (StageManager.IsTransitioning)
            return;

        // GAMEPLAY
        GlobalManager.Update();

        // Run gameplay loop
        GameCurrentLoop.Update();
        var notUpdatedEntities = EntityManager.GetAllEntities().Where(entity => entity.Updated == false).ToList();
        if (EntityManager.GetAllEntities().Any(entity => entity.Updated == false))
            Debugger.Break(); // All entities should be updated after creation, before being drawn

        // Check to trigger transitions
        StageManager.UpdatePlayerRoom();
        StageManager.PushPlayerInBounds();
        StageManager.CheckToTriggerSpawns();
    }

    public static void Draw()
    {
        if (GameCurrentLoop == null)
            return;

        DrawBackground();
        DrawAllLayers();
        StageEditor.Draw();
        DrawMasks();
    }

    private static void DrawBackground()
    {
        foreach (var room in StageManager.GetRoomsToDraw())
            room?.DrawBackground();
    }

    private static void DrawAllLayers()
    {
        foreach (var id in Enum.GetValues(typeof(LayerId)).Cast<int>()
                     .OrderByDescending(id => id))
            DrawLayer((LayerId)id);
    }

    private static void DrawLayer(LayerId layerId)
    {
        foreach (var room in StageManager.GetRoomsToDraw())
            room?.DrawTileLayer(layerId);
        EntityManager.GetAllEntities()
            .Where(entity => entity.LayerId == layerId)
            .OrderBy(entity => entity.EntityKind)
            .ThenBy(entity => entity.DrawOrder)
            .ToList()
            .ForEach(entity => entity.Draw());
    }

    private static void DrawMasks()
    {
        if (!DebugMode.ShowMasks)
            return;

        foreach (var entity in EntityManager.GetAllEntities())
        {
            entity.CollisionBox?.DrawBox();
            entity.Sprite?.DrawOrigin();
        }

        // Entity layouts
        StageManager.CurrentRoom.GetEntityLayout().DrawEntityLayout();
        StageManager.CurrentRoom.GetEntityLayout().DrawEntityLayoutCollisionBox();

        // Screen limits
        var spawnLimits = Camera.GetSpawnScreenLimits();
        Drawer.DrawRectangleOutline(spawnLimits.Position, spawnLimits.Size, CustomColor.TransparentWhite);
        var spawnResetLimits = Camera.GetSpawnScreenLimitsWithBorder(Settings.TileSize);
        Drawer.DrawRectangleOutline(spawnResetLimits.Position, spawnResetLimits.Size, CustomColor.TransparentWhite);
    }
}
