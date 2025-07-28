using Engine.Managers.StageHandling;
using Engine.Types;
using Microsoft.Xna.Framework.Input;

namespace Engine.Managers.StageEditing.Tools;

class StageEditorMoveRoomTool : StageEditorTool
{
    public override MouseCursor MouseCursor { get; } = MouseCursor.SizeAll;
    public override Input.Button Shortcut { get; } = Input.RoomMoveTool;
    private Room MovingRoom { get; set; }
    private IntVector2 MovingRoomClickOffset { get; set; } // Offset from the room's top left corner to the mouse position (in grid size)


    public override void Run()
    {
        if (!Input.ClickedOnGameScreen())
            return;

        if (Input.MouseLeftPressed)
        {
            MovingRoom = StageEditor.SelectedRoom;
            MovingRoomClickOffset = StageEditor.SelectionRoom - MovingRoom.Position;
        }
        var newPosition = StageEditor.SelectionRoom - MovingRoomClickOffset;

        if (!Input.MouseLeftHold || MovingRoom == null)
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
