using Engine.Helpers;
using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Managers.StageEditing.Tools;
using Engine.Managers.StageHandling;
using Engine.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers.StageEditing.Modes;

public class StageEditorTileMode : StageEditorMode
{
    public override Input.Button Shortcut { get; } = Input.TileMode;
    public Tileset CurrentTileset { get; set; }
    // Menu
    public IntVector2 SelectedMenuTile { get; private set; }
    private IntVector2 SelectionStartMenuTile { get; set; }
    private IntVector2 SelectionEndMenuTile { get; set; }

    // Game screen
    public IntVector2 SelectionStartGameTile { get; set; }
    public IntVector2 SelectionEndGameTile { get; set; }

    public StageEditorTileMode()
    {
        AvailableTools = new List<StageEditorTool>
        {
            new StageEditorBrushTileTool(),
            new StageEditorRectangleTileTool(),
        };
        SetTilesetOfType(GameManager.GameSpecificSettings.DefaultTilesetType);
    }

    public override void Run()
    {
        UpdateTilesetViaShortcut();
        TileEditorMenuClick();
        TileEditorCheckToResetCoordinates();
        CheckToToggleTools();
        CurrentTool?.Run();
    }

    private void TileEditorCheckToResetCoordinates()
    {
        if (!Input.MouseLeftReleased || !Input.MouseRightReleased)
            return;
        SelectionStartGameTile = IntVector2.New(-1, -1);
        SelectionEndGameTile = IntVector2.New(-1, -1);
    }

    public TileLayout GetCurrentTileLayout(int tileId)
    {
        var y = tileId / Settings.EditingMenuSizeInTiles.X;
        var layerId = CurrentTileset.GetIdFromLine(y);
        return StageEditor.SelectedRoom.GetTileLayout(layerId, CurrentTileset.GetType());
    }

    public IntVector2 GetSelectedTile()
    {
        return (Input.MousePositionOnGame - StageEditor.SelectedRoom.PositionInPixels).RoundDownToTileCoordinate();
    }

    public int GetTileSpriteId(IntVector2 gameTilePosition)
    {
        var loopingPosition = GetTileMenuPosition(gameTilePosition);
        return loopingPosition.Y * Settings.EditingMenuSizeInTiles.X + loopingPosition.X;
    }

    private IntVector2 GetTileMenuPosition(IntVector2 gameTilePosition)
    {
        var traveledDistance = gameTilePosition - SelectionStartGameTile; // Distance traveled drawing
        var stampOrigin = IntVector2.Min(SelectionStartMenuTile, SelectionEndMenuTile); // Top left corner
        var offset = SelectedMenuTile - stampOrigin; // Distance of selected tile to stamp origin
        var size = (SelectionEndMenuTile - SelectionStartMenuTile).Abs() + 1;

        // Ensure modulo is positive
        traveledDistance = (traveledDistance % size + size) % size;

        // Get looping position
        return stampOrigin + (traveledDistance + offset) % size;
    }

    private void TileEditorMenuClick()
    {
        if (!Input.ClickedOnEditingMenu())
            return;

        // Tile coordinates
        var tilePosition = Input.MousePositionOnMenu().RoundDownToTileCoordinate() / Settings.EditingMenuScale;

        // Select individual tile
        if (Input.MouseLeftPressed)
            SelectedMenuTile = tilePosition;

        // Select rectangle of tiles
        if (Input.Ctrl.Holding) // Hold Ctrl to select only the individual tile
            return;
        if (Input.MouseLeftHold)
            SelectionStartMenuTile = SelectedMenuTile;
        if (Input.MouseLeftHold)
            SelectionEndMenuTile = tilePosition;
    }

    private void UpdateTilesetViaShortcut()
    {
        Keys[] keys = { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0 };

        for (var i = 0; i < keys.Length; i++)
            if (Input.KeyboardState.IsKeyDown(keys[i]))
                SetTilesetOfIndex(i);
    }

    private void SetTilesetOfIndex(int index)
    {
        if (index >= GameManager.GameSpecificSettings.TilesetTypes.Count)
            return;
        var tilesetType = GameManager.GameSpecificSettings.TilesetTypes[index];
        if (tilesetType == null)
            return;
        SetTilesetOfType(tilesetType);
    }

    public void SetTilesetOfType(Type type) =>
        CurrentTileset = GetTilesetOfType(type);

    public Tileset GetTilesetOfType(Type type) =>
        StageManager.TilesetList.FirstOrDefault(tileset => tileset.GetType() == type);

    public override void Draw()
    {
        DrawTileGrid();
        CurrentTool?.Draw();

        // Draw selected tile
        var tileSelection = Input.MousePositionOnGame.RoundDownToTileCoordinate() * Settings.TileSize;
        Drawer.DrawRectangleOutline(tileSelection, Settings.TileSize, CustomColor.TransparentRed, 2);
    }

    private void DrawTileGrid()
    {
        // Draw tile outlines in all rooms
        foreach (var room in StageEditor.CurrentStage.RoomList)
        {
            for (var x = 0; x < room.SizeInTiles.Width; x++)
            {
                for (var y = 0; y < room.SizeInTiles.Height; y++)
                {
                    var tile = room.PositionInPixels + Settings.TileSize * (x, y);
                    if (Camera.GetDrawScreenLimits().Overlaps(new IntRectangle(tile, Settings.TileSize)))
                        Drawer.DrawRectangleOutline(tile, Settings.TileSize, CustomColor.TransparentGray);
                }
            }
        }
    }

    public override void DrawMenu()
    {
        // Draw tileset
        var texture = CurrentTileset.Texture;
        var sourceRectangle = texture.Bounds;
        var destinationSize = IntVector2.New(texture.Width, texture.Height) * Settings.EditingMenuScale;
        var destinationRectangle = new IntRectangle(IntVector2.Zero, destinationSize);
        Video.SpriteBatch.Draw(texture, destinationRectangle, sourceRectangle, CustomColor.White);

        // Draw debug tiles
        var debugWidth = texture.Width / Settings.TileSize.X;
        var debugHeight = CurrentTileset.GetTotalForegroundLines();
        for (var x = 0; x < debugWidth; x++)
        {
            for (var y = 0; y < debugHeight; y++)
            {
                var tilePosition = IntVector2.New(x, y) * Settings.TileSize * Settings.EditingMenuScale;
                var type = CurrentTileset.DebugMapper[x + y * debugWidth];
                var color = DebugTiles.GetTypeColor(type);
                Drawer.DrawRectangle(tilePosition, Settings.TileSize * Settings.EditingMenuScale, color);
            }
        }

        // Draw layer limits
        var layerPosition = IntVector2.Zero;
        foreach (var layer in CurrentTileset.LayerMapper)
        {
            var layerHeight = Settings.TileSize.Y * layer.lines;
            var layerSize = new IntVector2(CurrentTileset.Texture.Width, layerHeight) * Settings.EditingMenuScale;
            Drawer.DrawRectangleOutline(layerPosition, layerSize, CustomColor.Black);
            Drawer.DrawOutlinedString(Drawer.PicoFont, layer.id.ToString(), layerPosition + (1, 1), Color.White);
            layerPosition += new IntVector2(0, layerHeight) * Settings.EditingMenuScale;
        }

        // Draw selected tile
        var tileSize = Settings.TileSize * Settings.EditingMenuScale;
        var selectedTilePosition = SelectedMenuTile * tileSize;
        Drawer.DrawRectangleOutline(selectedTilePosition, tileSize, CustomColor.White);

        // Draw selected rectangle
        var rectanglePosition = IntVector2.Min(SelectionStartMenuTile, SelectionEndMenuTile) * tileSize;
        var rectangleSize = ((SelectionStartMenuTile - SelectionEndMenuTile).Abs() + 1) * tileSize;
        Drawer.DrawRectangleOutline(rectanglePosition, rectangleSize, CustomColor.White);
    }

}
