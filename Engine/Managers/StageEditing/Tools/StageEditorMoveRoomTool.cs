using Engine.Managers.Input;
using Engine.Managers.StageHandling;
using Engine.Types;
using Microsoft.Xna.Framework.Input;

namespace Engine.Managers.StageEditing.Tools;

class StageEditorMoveRoomTool : StageEditorTool
{
    public override MouseCursor MouseCursor { get; } = MouseCursor.SizeAll;
    public override Button Shortcut { get; } = EditorInput.RoomMoveTool;
    private Room MovingRoom { get; set; }
    private IntVector2 MovingRoomClickOffset { get; set; } // Offset from the room's top left corner to the mouse position (in grid size)


    public override void Run()
    {
        if (!MouseHandler.ClickedOnGameScreen())
            return;

        if (MouseHandler.MouseLeftPressed)
        {
            MovingRoom = StageEditor.SelectedRoom;
            MovingRoomClickOffset = StageEditor.SelectionRoom - MovingRoom.Position;
        }
        var newPosition = StageEditor.SelectionRoom - MovingRoomClickOffset;

        if (!MouseHandler.MouseLeftHold || MovingRoom == null)
            return;
        if (!StageEditor.RoomMode.RoomSpaceIsFree(MovingRoom.Size, newPosition, MovingRoom))
            return;

        MovingRoom.RemoveRoomFromGrid();
        MovingRoom.Position = newPosition;
        MovingRoom.AddRoomToGrid();
    }

    public override void Draw()
    {
    }
}
