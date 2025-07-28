using Engine.Helpers;
using Engine.Main;
using Engine.Types;
using System.Collections.Generic;

namespace Engine.Managers.StageHandling;

public class Stage
{
    public IntVector2 PlayerStartingPosition { get; set; }
    public Room[,] RoomGrid { get; private set; } // used to find rooms by coordinates
    public List<Room> RoomList { get; } = new(); // used to save each room once
    public IntVector2 RespawnPosition { get; set; }

    public Stage(IntVector2 size)
    {
        RoomGrid = new Room[size.Width, size.Height];
    }

    public bool HasRoomAt(IntVector2 position)
    {
        return GetRoomAtGrid(position) != null;
    }

    public Room GetRoomAtGrid(IntVector2 gridPosition)
    {
        return RoomGrid.GetValueAt(gridPosition);
    }

    public Room GetRoomAtPixel(IntVector2 position)
    {
        if (position.X < 0 || position.Y < 0)
            return null;
        var gridPosition = position / Settings.RoomSizeInPixels;
        return GetRoomAtGrid(gridPosition);
    }

    public Room AddNewRoom(IntVector2 gridPosition)
    {
        if (gridPosition.X < 0 || gridPosition.Y < 0)
            return null;

        // Create room and add to list/grid
        var room = new Room(this, gridPosition);
        RoomList.Add(room);
        SetValueAtRoomGridPosition(gridPosition, room);
        return room;
    }

    public void SetValueAtRoomGridPosition(IntVector2 gridPosition, Room room)
    {
        RoomGrid = RoomGrid.SetValueExpandIfOutOfBounds(gridPosition, room);
    }

    public void RemoveRoomAt(IntVector2 gridPosition)
    {
        var roomToRemove = GetRoomAtGrid(gridPosition);
        RemoveRoom(roomToRemove);
    }

    public void RemoveRoom(Room room)
    {
        room.DestroyEntities();
        room.RemoveRoomFromGrid();
        RoomList.Remove(room);
    }
}
