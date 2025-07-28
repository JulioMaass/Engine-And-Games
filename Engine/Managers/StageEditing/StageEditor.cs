using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Managers.SaveSystem;
using Engine.Managers.StageEditing.Modes;
using Engine.Managers.StageHandling;
using Engine.Types;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers.StageEditing;

public static class StageEditor
{
    public static Stage CurrentStage => StageManager.CurrentStage;
    public static bool IsOn { get; private set; }
    public static IntVector2 SelectionRoom => Input.MousePositionOnGame / Settings.RoomSizeInPixels;
    public static Room SelectedRoom => StageManager.CurrentStage.GetRoomAtGrid(SelectionRoom);

    // Modes
    public static StageEditorEntityMode EntityMode { get; } = new();
    public static StageEditorTileMode TileMode { get; } = new();
    public static StageEditorRoomMode RoomMode { get; } = new();
    public static StageEditorMode CurrentMode { get; set; }
    public static List<StageEditorMode> Modes { get; } = new() { TileMode, EntityMode, RoomMode };

    public static void Update()
    {
        CheckToTurnOnAndOff();
        StringInput.CheckToTurnOff();
        CheckToSaveStages();
        if (!IsOn)
            return;
        CheckToChangeMode();
        RunEditor();
    }

    private static void CheckToTurnOnAndOff()
    {
        if (!Input.CtrlCommand(Input.Edit))
            return;
        if (IsOn)
            TurnOff();
        else
            TurnOn();
    }

    private static void TurnOn()
    {
        if (CurrentMode == null)
            ChangeMode(TileMode);
        IsOn = true;
        Video.ResizeScreen(Settings.ScreenScaledSize + (Settings.EditingMenuWidth, 0));
    }

    private static void TurnOff()
    {
        IsOn = false;
        Video.ResizeScreen(Settings.ScreenScaledSize);

        if (EntityManager.PlayerEntity == null)
            StageManager.CurrentRoom = StageManager.GetStartingRoom();
        Camera.UpdatePanning();

        DeleteEmptyLayers();
    }

    private static void DeleteEmptyLayers()
    {
        foreach (var room in CurrentStage.RoomList)
            foreach (var layer in room.Layers.ToList())
            {
                if (layer is not TileLayout tileLayout)
                    continue;
                if (tileLayout.IsEmpty())
                    room.Layers.Remove(layer);
            }
    }

    private static void CheckToSaveStages()
    {
        if (!Input.CtrlCommand(Input.Save))
            return;
        var stageData = new StageData();
        DeleteEmptyLayers();
        stageData.SaveStageData(CurrentStage);
        JsonHandler.SaveObjectToFile(stageData, StageManager.CurrentStageName);
    }

    private static void CheckToChangeMode()
    {
        foreach (var mode in Modes)
            if (Input.ShortcutCommand(mode.Shortcut))
                ChangeMode(mode);

        if (Input.CtrlCommand(Input.LaptopMode))
            Settings.LaptopModeIsOn = !Settings.LaptopModeIsOn;
    }

    private static void ChangeMode(StageEditorMode mode)
    {
        CurrentMode = mode;
        CurrentMode.ToggleTool(CurrentMode.DefaultTool);
    }

    private static void RunEditor()
    {
        if (Input.Panning.Holding)
            return;
        CurrentMode.Run();
    }

    public static void Draw()
    {
        if (!IsOn)
            return;
        DrawRoomsDebug();
        DrawRoomGrid();
        DrawAllRoomOutlines();
        CurrentMode.Draw();
        DrawEntityLayouts();
        DrawStageOutline();
    }

    private static void DrawRoomsDebug()
    {
        foreach (var room in StageManager.GetRoomsToDraw())
        {
            room?.DebugTiles.Draw();
            room?.DrawLayerData();
        }
    }

    private static void DrawRoomGrid()
    {
        for (var x = 0; x < CurrentStage.RoomGrid.GetLength(0); x++)
        {
            for (var y = 0; y < CurrentStage.RoomGrid.GetLength(1); y++)
            {
                var position = (x, y) * Settings.RoomSizeInPixels;
                if (Camera.GetDrawScreenLimits().Overlaps(new IntRectangle(position, Settings.RoomSizeInPixels)))
                    Drawer.DrawRectangleOutline(position, Settings.RoomSizeInPixels, CustomColor.TransparentWhite);
            }
        }
    }

    private static void DrawAllRoomOutlines()
    {
        foreach (var room in CurrentStage.RoomList)
            if (Camera.GetDrawScreenLimits().Overlaps(new IntRectangle(room.PositionInPixels, room.SizeInPixels)))
                Drawer.DrawRectangleOutline(room.PositionInPixels, room.SizeInPixels, CustomColor.TransparentGreen, 2);
    }

    private static void DrawStageOutline()
    {
        var stageWidth = CurrentStage.RoomGrid.GetLength(0) * Settings.RoomSizeInPixels.Width;
        var stageHeight = CurrentStage.RoomGrid.GetLength(1) * Settings.RoomSizeInPixels.Height;
        var stageSize = IntVector2.New(stageWidth, stageHeight);
        Drawer.DrawRectangleOutline(IntVector2.Zero, stageSize, CustomColor.TransparentWhite);
    }

    private static void DrawEntityLayouts()
    {
        foreach (var room in CurrentStage.RoomList)
            room.GetEntityLayout().DrawEntityLayout();
    }

    public static void DrawMenu()
    {
        if (!IsOn)
            return;
        if (TileMode.CurrentTileset.Texture == null)
            return;
        var backgroundSize = IntVector2.New(TileMode.CurrentTileset.Texture.Width, TileMode.CurrentTileset.Texture.Height) / Settings.TileSize;
        Drawer.DrawEmptyBackground(IntVector2.Zero, backgroundSize, Settings.EditingMenuScale);

        CurrentMode.DrawMenu();
    }
}