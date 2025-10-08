using Engine.Helpers;
using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Managers.Input;
using Engine.Managers.StageHandling;
using Engine.Types;
using Microsoft.Xna.Framework.Input;

namespace Engine.Managers.StageEditing.Tools;

class StageEditorRectangleTileTool : StageEditorTool
{
    public override MouseCursor MouseCursor { get; } = MouseCursor.SizeNWSE;
    public override Button Shortcut { get; } = EditorInput.TileRectangleTool;


    public override void Run()
    {
        if (StageEditor.SelectedRoom == null)
            return;
        TileEditorUpdateRectangleToolSelection();
        TileEditorGameScreenRelease();
    }

    private void TileEditorUpdateRectangleToolSelection()
    {
        if (!MouseHandler.ClickedOnGameScreen())
            return;

        // Get tile coordinates
        var tilePosition = StageEditor.TileMode.GetSelectedTile();
        if (MouseHandler.MouseLeftPressed || MouseHandler.MouseRightPressed)
            StageEditor.TileMode.SelectionStartGameTile = tilePosition;
        if (MouseHandler.MouseLeftHold || MouseHandler.MouseRightHold)
            StageEditor.TileMode.SelectionEndGameTile = tilePosition;
    }

    private void TileEditorGameScreenRelease()
    {
        if (!MouseHandler.MouseIsOnGameScreen())
            return;
        if (!MouseHandler.MouseLeftReleased && !MouseHandler.MouseRightReleased)
            return;

        // Rectangle limits
        var min = IntVector2.Min(StageEditor.TileMode.SelectionStartGameTile, StageEditor.TileMode.SelectionEndGameTile);
        var max = IntVector2.Max(StageEditor.TileMode.SelectionStartGameTile, StageEditor.TileMode.SelectionEndGameTile);

        for (var x = min.X; x <= max.X; x++)
        {
            for (var y = min.Y; y <= max.Y; y++)
            {
                var position = IntVector2.New(x, y);
                var tileSpriteId = StageEditor.TileMode.GetTileSpriteId(position);
                var tileLayout = StageEditor.TileMode.GetCurrentTileLayout(tileSpriteId);

                // Draw/erase tiles
                if (MouseHandler.MouseLeftReleased)
                    tileLayout.SetTileAt(position, tileSpriteId);
                else
                    tileLayout.SetTileAt(position, TileLayout.EMPTY);
            }
        }
    }

    public override void Draw()
    {
        if (!MouseHandler.ClickedOnGameScreen())
            return;
        if (EditorInput.Panning.Holding)
            return;
        if (!MouseHandler.MouseLeftHold && !MouseHandler.MouseRightHold)
            return;
        if (StageEditor.SelectedRoom == null)
            return;

        // Rectangle limits
        var min = IntVector2.Min(StageEditor.TileMode.SelectionStartGameTile, StageEditor.TileMode.SelectionEndGameTile);
        var max = IntVector2.Max(StageEditor.TileMode.SelectionStartGameTile, StageEditor.TileMode.SelectionEndGameTile);

        for (var x = min.X; x <= max.X; x++)
        {
            for (var y = min.Y; y <= max.Y; y++)
            {
                // Get source rectangle
                var tilePosition = IntVector2.New(x, y);
                var tileSpriteId = StageEditor.TileMode.GetTileSpriteId(tilePosition);
                var sourceRectangle = Drawer.GetSourceRectangleFromId(StageEditor.TileMode.CurrentTileset.Texture.Width, IntVector2.Zero, Settings.TileSize, tileSpriteId);

                // Draw tile
                var pixelPosition = IntVector2.New(x, y) * Settings.TileSize + StageEditor.SelectedRoom.PositionInPixels;
                Drawer.DrawRectangle(pixelPosition, Settings.TileSize, CustomColor.Black); // Draw black background
                if (MouseHandler.MouseLeftHold)
                    Drawer.DrawTextureRectangleAt(StageEditor.TileMode.CurrentTileset.Texture, sourceRectangle, pixelPosition); // Draw tile
            }
        }
    }
}
