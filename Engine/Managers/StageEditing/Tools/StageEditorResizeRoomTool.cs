using Engine.Managers.Graphics;
using Engine.Managers.Input;
using Engine.Types;
using Microsoft.Xna.Framework.Input;

namespace Engine.Managers.StageEditing.Tools;

class StageEditorResizeRoomTool : StageEditorTool
{
    public override MouseCursor MouseCursor { get; } = MouseCursor.FromTexture2D(Drawer.TextureDictionary["CursorAddRight"], 16, 16);
    public override Button Shortcut { get; } = EditorInput.RoomResizeTool;

    public override void Run()
    {
        UpdateResizeToolCursor();

        if (!MouseHandler.ClickedOnGameScreen())
            return;

        if (MouseHandler.MouseLeftPressed)
            RoomEditorLeftClick();

        if (MouseHandler.MouseRightPressed && StageEditor.CurrentStage.HasRoomAt(StageEditor.SelectionRoom))
            RoomEditorRightClick();
    }

    private void RoomEditorLeftClick()
    {
        if (StageEditor.CurrentStage.HasRoomAt(StageEditor.SelectionRoom))
            IncreaseRoomAtSelection();
        else
            StageEditor.CurrentStage.AddNewRoom(StageEditor.SelectionRoom);
    }

    private void RoomEditorRightClick()
    {
        // Calculate room size change
        var sizeIncrease = IntVector2.Zero;
        if (InputHandler.Shift.Holding)
            sizeIncrease += (0, -1);
        else
            sizeIncrease += (-1, 0);

        // Calculate new position and resizing direction
        var dir = 1;
        var newPosition = StageEditor.SelectedRoom.Position;
        if (InputHandler.Ctrl.Holding)
        {
            dir = -1;
            newPosition += sizeIncrease * -1;
        }

        // Remove room if it would be 0 size, otherwise resize room
        if (StageEditor.SelectedRoom.Size.Height + sizeIncrease.Height == 0 || StageEditor.SelectedRoom.Size.Width + sizeIncrease.Width == 0)
            StageEditor.CurrentStage.RemoveRoomAt(StageEditor.SelectionRoom);
        else
            StageEditor.SelectedRoom.ChangeRoomSize(sizeIncrease, newPosition, dir);
    }

    private void IncreaseRoomAtSelection()
    {
        // Calculate room size change
        var sizeIncrease = IntVector2.Zero;
        if (InputHandler.Shift.Holding)
            sizeIncrease += (0, 1);
        else
            sizeIncrease += (1, 0);
        var newSize = StageEditor.SelectedRoom.Size + sizeIncrease;

        // Calculate new position and resizing direction
        var dir = 1;
        var newPosition = StageEditor.SelectedRoom.Position;
        if (InputHandler.Ctrl.Holding)
        {
            dir = -1;
            newPosition += sizeIncrease * -1;
        }

        // Check if space is occupied by another room before increasing size
        if (!StageEditor.RoomMode.RoomSpaceIsFree(newSize, newPosition, StageEditor.SelectedRoom))
            return;

        // Increase grid and room size
        StageEditor.SelectedRoom.ChangeRoomSize(sizeIncrease, newPosition, dir);
    }

    private void UpdateResizeToolCursor()
    {
        var cursorTextureName = "";
        if (!InputHandler.Shift.Holding && !InputHandler.Ctrl.Holding)
            cursorTextureName = "CursorAddRight";
        else if (InputHandler.Shift.Holding && !InputHandler.Ctrl.Holding)
            cursorTextureName = "CursorAddDown";
        else if (!InputHandler.Shift.Holding && InputHandler.Ctrl.Holding)
            cursorTextureName = "CursorAddLeft";
        else if (InputHandler.Shift.Holding && InputHandler.Ctrl.Holding)
            cursorTextureName = "CursorAddUp";
        Mouse.SetCursor(MouseCursor.FromTexture2D(Drawer.TextureDictionary[cursorTextureName], 16, 16));
    }

    public override void Draw()
    {
    }
}
