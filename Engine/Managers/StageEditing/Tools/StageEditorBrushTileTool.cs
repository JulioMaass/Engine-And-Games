using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;
using Engine.Types;
using Microsoft.Xna.Framework.Input;

namespace Engine.Managers.StageEditing.Tools;

class StageEditorBrushTileTool : StageEditorTool
{
    public override MouseCursor MouseCursor { get; } = MouseCursor.Arrow;
    public override Input.Button Shortcut { get; } = Input.TileBrushTool;


    public override void Run()
    {
        if (!Input.ClickedOnGameScreen())
            return;
        if (StageEditor.SelectedRoom == null)
            return;

        TileEditorBrushTool();
    }

    private void TileEditorBrushTool()
    {
        // Get tile coordinates
        var tilePosition = StageEditor.TileMode.GetSelectedTile();
        if (Input.MouseLeftPressed)
            StageEditor.TileMode.SelectionStartGameTile = tilePosition;
        if (Input.MouseLeftHold)
            StageEditor.TileMode.SelectionEndGameTile = tilePosition;

        var tileSpriteId = StageEditor.TileMode.GetTileSpriteId(StageEditor.TileMode.SelectionEndGameTile);
        var tileLayout = StageEditor.TileMode.GetCurrentTileLayout(tileSpriteId);

        if (Input.MouseLeftHold)
            tileLayout.SetTileAt(tilePosition, tileSpriteId);
        if (Input.MouseRightHold)
            tileLayout.SetTileAt(tilePosition, TileLayout.EMPTY);
    }

    public override void Draw()
    {
        if (Input.MouseLeftHold || Input.MouseRightHold) return;
        if (StageEditor.SelectedRoom == null) return;

        var sourceRectangle = new IntRectangle(StageEditor.TileMode.SelectedMenuTile * Settings.TileSize, Settings.TileSize);
        var tileSelection = Input.MousePositionOnGame.RoundDownToTileCoordinate() * Settings.TileSize;
        Drawer.DrawTextureRectangleAt(StageEditor.TileMode.CurrentTileset.Texture, sourceRectangle, tileSelection);
    }
}
