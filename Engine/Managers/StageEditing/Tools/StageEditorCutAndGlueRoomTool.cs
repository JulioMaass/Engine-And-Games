using Engine.Helpers;
using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Managers.Input;
using Engine.Types;
using Microsoft.Xna.Framework.Input;

namespace Engine.Managers.StageEditing.Tools;

class StageEditorCutAndGlueRoomTool : StageEditorTool
{
    public override MouseCursor MouseCursor { get; } = MouseCursor.FromTexture2D(Drawer.TextureDictionary["CursorSplit"], 16, 16);
    public override Button Shortcut { get; } = EditorInput.RoomCutAndGlueTool;

    public override void Run()
    {
        if (!MouseHandler.ClickedOnGameScreen())
            return;

        if (StageEditor.SelectedRoom == null)
            return;
        var cutPositionX = GetCutPositionX();

        if (MouseHandler.MouseLeftPressed)
            StageEditor.SelectedRoom.CutRoom(cutPositionX);
        else if (MouseHandler.MouseRightPressed)
            StageEditor.SelectedRoom.GlueRoom();
    }

    private int GetCutPositionX() // Ranges from 1 to room width
        => StageEditor.SelectionRoom.X - StageEditor.SelectedRoom.Position.X + 1;

    public override void Draw()
    {
        var thickness = 4;
        var cutPositionX = GetCutPositionX();
        var cutPosition = StageEditor.SelectedRoom.PositionInPixels + (cutPositionX * Settings.RoomSizeInPixels.Width - thickness / 2, 0);
        Drawer.DrawRectangle(cutPosition, (thickness, StageEditor.SelectedRoom.SizeInPixels.Height), CustomColor.TransparentBlue);
    }
}
