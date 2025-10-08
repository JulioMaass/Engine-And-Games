using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Managers.Input;
using Engine.Managers.StageHandling;
using Engine.Types;
using Microsoft.Xna.Framework.Input;

namespace Engine.Managers.StageEditing.Tools;

class StageEditorBrushTileTool : StageEditorTool
{
    public override MouseCursor MouseCursor { get; } = MouseCursor.Arrow;
    public override Button Shortcut { get; } = EditorInput.TileBrushTool;


    public override void Run()
    {
        if (!MouseHandler.ClickedOnGameScreen())
            return;
        if (StageEditor.SelectedRoom == null)
            return;

        TileEditorBrushTool();
    }

    private void TileEditorBrushTool()
    {
        // Get tile coordinates
        var tilePosition = StageEditor.TileMode.GetSelectedTile();
        if (MouseHandler.MouseLeftPressed)
            StageEditor.TileMode.SelectionStartGameTile = tilePosition;
        if (MouseHandler.MouseLeftHold)
            StageEditor.TileMode.SelectionEndGameTile = tilePosition;

        var tileSpriteId = StageEditor.TileMode.GetTileSpriteId(StageEditor.TileMode.SelectionEndGameTile);
        var tileLayout = StageEditor.TileMode.GetCurrentTileLayout(tileSpriteId);

        if (MouseHandler.MouseLeftHold)
            tileLayout.SetTileAt(tilePosition, tileSpriteId);
        if (MouseHandler.MouseRightHold)
            tileLayout.SetTileAt(tilePosition, TileLayout.EMPTY);
    }

    public override void Draw()
    {
        if (MouseHandler.MouseLeftHold || MouseHandler.MouseRightHold) return;
        if (StageEditor.SelectedRoom == null) return;

        var sourceRectangle = new IntRectangle(StageEditor.TileMode.SelectedMenuTile * Settings.TileSize, Settings.TileSize);
        var tileSelection = MouseHandler.MousePositionOnGame.RoundDownToTileCoordinate() * Settings.TileSize;
        Drawer.DrawTextureRectangleAt(StageEditor.TileMode.CurrentTileset.Texture, sourceRectangle, tileSelection);
    }
}
