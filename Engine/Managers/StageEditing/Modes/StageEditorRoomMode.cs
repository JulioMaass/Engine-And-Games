using Engine.Helpers;
using Engine.Managers.Graphics;
using Engine.Managers.Input;
using Engine.Managers.StageEditing.Tools;
using Engine.Managers.StageHandling;
using Engine.Types;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers.StageEditing.Modes;

public class StageEditorRoomMode : StageEditorMode
{
    public override Button Shortcut { get; } = EditorInput.RoomMode;
    public StageEditorRoomMode()
    {
        AvailableTools = new List<StageEditorTool>
        {
            new StageEditorResizeRoomTool(),
            new StageEditorMoveRoomTool(),
            new StageEditorCutAndGlueRoomTool(),
        };
    }

    public override void Run()
    {
        CheckToToggleTools();
        CurrentTool?.Run();
    }

    public bool RoomSpaceIsFree(IntVector2 size, IntVector2 newPosition, Room ignoredRoom)
    {
        var rooms = GetRoomsInArea(size, newPosition);
        rooms.Remove(ignoredRoom);
        return rooms.Count == 0;
    }

    private List<Room> GetRoomsInArea(IntVector2 size, IntVector2 newPosition)
    {
        var rooms = new List<Room>();
        for (var x = 0; x < size.Width; x++)
        {
            for (var y = 0; y < size.Height; y++)
            {
                var gridPosition = newPosition + (x, y);
                var room = StageEditor.CurrentStage.GetRoomAtGrid(gridPosition);
                if (room != null)
                    rooms.Add(room);
            }
        }
        return rooms.Distinct().ToList();
    }

    public override void Draw()
    {
        // Draw room selection
        if (StageEditor.SelectedRoom != null)
            Drawer.DrawRectangleOutline(StageEditor.SelectedRoom.PositionInPixels, StageEditor.SelectedRoom.SizeInPixels, CustomColor.TransparentRed, 2);
    }

    public override void DrawMenu()
    {

    }
}
