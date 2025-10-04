using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Managers.StageEditing;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Engine.Managers.StageHandling;

public class Room
{
    private Stage Stage { get; }
    public IntVector2 Position { get; set; }
    public IntVector2 PositionInTiles => Position * Settings.RoomSizeInTiles;
    public IntVector2 PositionInPixels => Position * Settings.RoomSizeInPixels;
    public IntVector2 Size { get; set; }
    public IntVector2 SizeInTiles => Size * Settings.RoomSizeInTiles;
    public IntVector2 SizeInPixels => Size * Settings.RoomSizeInPixels;

    // Layers
    public List<Layer> Layers { get; } = new();
    public DebugTiles DebugTiles { get; private set; }

    public Room(Stage stage, IntVector2 position)
    {
        Stage = stage;
        Position = position;
        Size = IntVector2.New(1, 1);
        GenerateDebugLayer();
    }

    public void AddLayer(Layer layer)
    {
        Layers.Add(layer);
        DebugTiles.UpdateLayout();
    }

    public void RemoveLayer(Layer layer)
    {
        Layers.Remove(layer);
        DebugTiles.UpdateLayout();
    }

    public void GenerateDebugLayer()
    {
        DebugTiles = new(this);
    }

    public TileLayout GetTileLayout(LayerId layerId, Type tilesetType)
    {
        var layout = Layers.FirstOrDefault(layer => layer is TileLayout tileLayout && tileLayout.LayerId == layerId && tileLayout.Tileset.GetType() == tilesetType);
        if (layout == null)
        {
            layout = new TileLayout(this, tilesetType, layerId);
            ((TileLayout)layout).CreateTileLayout();
            AddLayer(layout);
        }
        return (TileLayout)layout;
    }

    public EntityLayout GetEntityLayout()
    {
        var layout = Layers.FirstOrDefault(layer => layer is EntityLayout);
        if (layout == null)
        {
            layout = new EntityLayout(this);
            AddLayer(layout);
        }
        return (EntityLayout)layout;
    }

    public void RemoveTileAtLayer(IntVector2 position, LayerId layerId)
    {
        foreach (var tileLayout in Layers.OfType<TileLayout>()
                     .Where(tileLayout => tileLayout.LayerId == layerId).ToList())
            tileLayout.Layout.SetValueAt(position, TileLayout.EMPTY);
    }

    public void DestroyTileAt(IntVector2 position)
    {
        foreach (var tileLayout in DebugTiles.ForegroundTileLayouts)
        {
            var tileId = tileLayout.GetValueAt(position);
            tileLayout.CheckToDropItem(tileId, position);
            tileLayout.Layout.SetValueAt(position, TileLayout.EMPTY);
        }
    }

    public void ChangeRoomSize(IntVector2 sizeIncrease) =>
        ChangeRoomSize(sizeIncrease, Position, 1);

    public void ChangeRoomSize(IntVector2 sizeIncrease, IntVector2 newPosition, int dir)
    {
        RemoveRoomFromGrid();
        Position = newPosition;

        // Increase size
        Size += sizeIncrease;
        var tileSize = Size * Settings.RoomSizeInTiles;

        // Offset all tile layouts
        var offset = IntVector2.Zero;
        if (dir < 0)
            offset = sizeIncrease * Settings.RoomSizeInTiles;

        // Resize tile and entity layouts
        foreach (var layer in Layers)
        {
            if (layer is TileLayout tileLayout)
                tileLayout.ResizeWithOffsetData(tileSize, offset);
            else if (layer is EntityLayout entityLayout)
            {
                foreach (var entityInstance in entityLayout.List.ToList())
                {
                    entityInstance.PositionOnRoom += offset * Settings.TileSize;
                    if (!RelativePositionIsInsideRoom(entityInstance.PositionOnRoom))
                        GetEntityLayout().List.Remove(entityInstance);
                }
            }
        }
        DebugTiles = new DebugTiles(this);
        AddRoomToGrid();
    }

    public void CutRoom(int cutPositionX)
    {
        if (Size.Width <= 1)
            return;
        if (cutPositionX <= 0 || cutPositionX >= Size.Width)
            return;

        Stage.RemoveRoom(this);

        var newRoomLeft = CloneRoom();
        var newRoomRight = CloneRoom();

        newRoomLeft.ChangeRoomSize(new IntVector2(-Size.Width + cutPositionX, 0), Position, 1);
        newRoomRight.ChangeRoomSize(new IntVector2(-cutPositionX, 0), Position + (cutPositionX, 0), -1);

        newRoomLeft.AddRoomToGrid();
        newRoomRight.AddRoomToGrid();

        Stage.RoomList.Add(newRoomLeft);
        Stage.RoomList.Add(newRoomRight);
    }

    public void GlueRoom() // Merge room with the room to the right
    {
        var roomToTheRight = Stage.GetRoomAtGrid(Position + (Size.Width, 0));
        if (roomToTheRight == null)
            return;
        if (Size.Height != roomToTheRight.Size.Height || Position.Y != roomToTheRight.Position.Y)
            return;

        Stage.RemoveRoom(roomToTheRight);
        var oldSizeInTiles = SizeInTiles;
        var newSize = Size + (roomToTheRight.Size.Width, 0);
        ChangeRoomSize(newSize - Size, Position, 1);

        foreach (var layer in roomToTheRight.Layers)
        {
            if (layer is TileLayout roomToTheRightTileLayout)
            {
                if (roomToTheRightTileLayout.Layout == null)
                    continue;
                var originalTileLayout = GetTileLayout(roomToTheRightTileLayout.LayerId, roomToTheRightTileLayout.Tileset.GetType());
                originalTileLayout.Layout.PasteArrayAt(roomToTheRightTileLayout.Layout, (oldSizeInTiles.X, 0));
            }
            else if (layer is EntityLayout roomToTheRightEntityLayout)
            {
                var originalEntityLayout = GetEntityLayout();
                foreach (var entityInstance in roomToTheRightEntityLayout.List)
                {
                    var positionOnRoom = entityInstance.PositionOnRoom + (oldSizeInTiles.X * Settings.TileSize.X, 0);
                    var copiedEntityInstance = new EntityInstance(originalEntityLayout, entityInstance.EntityType, positionOnRoom);
                    originalEntityLayout.List.Add(copiedEntityInstance);
                }
            }
        }
    }

    private Room CloneRoom()
    {
        var newRoom = new Room(Stage, Position);
        newRoom.Size = Size;
        foreach (var layer in Layers)
        {
            if (layer is TileLayout tileLayout)
            {
                var newTileLayout = newRoom.GetTileLayout(tileLayout.LayerId, tileLayout.Tileset.GetType());
                newTileLayout.CopyLayout(tileLayout.Layout);
            }
            else if (layer is EntityLayout entityLayout)
            {
                var newEntityLayout = new EntityLayout(newRoom);
                foreach (var entityInstance in entityLayout.List)
                {
                    var newEntityInstance = new EntityInstance(newEntityLayout, entityInstance.EntityType, entityInstance.PositionOnRoom);
                    newEntityLayout.List.Add(newEntityInstance);
                }
                newRoom.Layers.Add(newEntityLayout);
            }
        }
        return newRoom;
    }

    private bool RelativePositionIsInsideRoom(IntVector2 positionOnRoom)
    {
        var roomRectangle = new IntRectangle(IntVector2.Zero, SizeInPixels);
        return roomRectangle.Contains(positionOnRoom);
    }

    public void AddRoomToGrid() // Adds all positions of the room to RoomGrid
    {
        for (var x = 0; x < Size.Width; x++)
            for (var y = 0; y < Size.Height; y++)
                Stage.SetValueAtRoomGridPosition(Position + (x, y), this);
    }

    public void RemoveRoomFromGrid() // Removes all positions of the room from RoomGrid
    {
        for (var x = 0; x < Size.Width; x++)
            for (var y = 0; y < Size.Height; y++)
                Stage.RoomGrid.SetValueAt(Position + (x, y), null);
    }

    public void DestroyEntities()
    {
        foreach (var entity in EntityManager.GetAllEntities()
            .Where(entity => entity.SpawnManager.Room == this))
            EntityManager.DeleteEntity(entity);
    }

    public void ResetAllSpawners()
    {
        foreach (var entityInstance in GetEntityLayout().List)
            entityInstance.CanSpawn = true;
    }

    public void DrawBackground()
    {
        if (StageEditor.IsOn)
            DrawEmptyBackground();
        else
            DrawBackgroundColor();
    }

    public void DrawBackgroundColor() =>
        Drawer.DrawRectangle(PositionInPixels, SizeInPixels, Drawer.BackgroundColor);

    public void DrawEmptyBackground()
    {
        if (!Camera.DrawScreenLimits.Overlaps(new IntRectangle(PositionInPixels, SizeInPixels)))
            return;
        Drawer.DrawEmptyBackground(PositionInTiles, SizeInTiles);
    }

    public void DrawTileLayer(LayerId layerId)
    {
        foreach (var tileLayer in Layers.OfType<TileLayout>().Where(l => l.LayerId == layerId))
            tileLayer.Draw();
    }

    public void DrawLayerData()
    {
        var line = 0;
        foreach (var layer in Layers)
        {
            if (layer is TileLayout tileLayout)
            {
                var texture = Path.GetFileName(tileLayout.Tileset.Texture.Name);
                var layerId = tileLayout.LayerId.ToString();
                Drawer.DrawOutlinedString(Drawer.PicoFont, $"{texture} {layerId}", PositionInPixels + (0, line * 8), Color.White);
                line++;
            }
            else if (layer is EntityLayout entityLayout)
            {
                var totalEntities = entityLayout.List.Count;
                Drawer.DrawOutlinedString(Drawer.PicoFont, $"Entities: {totalEntities}", PositionInPixels + (0, line * 8), Color.White);
                line++;
            }
        }
    }
}
