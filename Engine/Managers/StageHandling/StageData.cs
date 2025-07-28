using Engine.ECS.Entities;
using Engine.GameSpecific;
using Engine.Main;
using Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace Engine.Managers.StageHandling;

public class StageData
{
    private Stage Stage { get; set; }

    public List<RoomData> Rooms { get; set; }

    public class RoomData
    {
        public IntVector2Nullable Position { get; set; }
        public IntVector2Nullable Size { get; set; }
        public List<LayerData> Layers { get; set; }
    }

    public class LayerData
    {
        public LayerType LayerType { get; set; }
        public TileData Tiles { get; set; }
        public List<EntityData> Entities { get; set; }
    }

    public class TileData
    {
        public int[,] Layout { get; set; }
        public string TilesetType { get; set; }
        public string LayerId { get; set; }
    }

    public class EntityData
    {
        public string EntityType { get; set; }
        public IntVector2Nullable Position { get; set; }
        public List<int> CustomValues { get; set; }
    }

    public void SaveStageData(Stage stage)
    {
        Rooms = new();
        foreach (var room in stage.RoomList)
        {
            var roomData = new RoomData
            {
                Position = IntVector2Nullable.New(room.Position.X, room.Position.Y),
                Size = IntVector2Nullable.New(room.Size.X, room.Size.Y),
                Layers = new()
            };
            SaveRoomData(room, roomData);
            Rooms.Add(roomData);
        }
    }

    private void SaveRoomData(Room room, RoomData roomData)
    {
        foreach (var layer in room.Layers)
        {
            var layerData = new LayerData();
            if (layer is TileLayout tileLayout)
            {
                layerData.LayerType = LayerType.Tile;
                layerData.Tiles = new TileData();
                SaveTileData(tileLayout, layerData.Tiles);
            }
            else if (layer is EntityLayout entityLayout)
            {
                layerData.LayerType = LayerType.Entity;
                layerData.Entities = new();
                SaveEntityData(entityLayout, layerData.Entities);
            }
            if (layerData.Tiles != null || layerData.Entities != null)
                roomData.Layers.Add(layerData);
        }
    }

    private void SaveTileData(TileLayout tileLayout, TileData tileData)
    {
        var layout = tileLayout.GetLayoutCopy();
        tileData.Layout = layout;
        tileData.TilesetType = tileLayout.Tileset.GetType().Name;
        tileData.LayerId = tileLayout.LayerId.ToString();
    }

    private void SaveEntityData(EntityLayout entityLayout, List<EntityData> entitiesData)
    {
        foreach (var entityInstance in entityLayout.List)
        {
            var entityData = new EntityData
            {
                EntityType = entityInstance.EntityType.Name,
                Position = IntVector2Nullable.New(entityInstance.PositionOnRoom.X, entityInstance.PositionOnRoom.Y),
                CustomValues = entityInstance.CustomValues.ToList(),
            };
            if (entityInstance.CustomValues.Count == 0)
                entityData.CustomValues = null;
            entitiesData.Add(entityData);
        }
    }

    public Stage LoadStageData()
    {
        Stage = new Stage(IntVector2.Zero);
        foreach (var roomData in Rooms)
        {
            var room = Stage.AddNewRoom(IntVector2Nullable.ToIntVector2(roomData.Position));
            if (roomData.Size != null)
                room.ChangeRoomSize(IntVector2Nullable.ToIntVector2(roomData.Size) - (1, 1));

            LoadRoomData(room, roomData);
            room.GenerateDebugLayer();
        }
        return Stage;
    }

    private void LoadRoomData(Room room, RoomData roomData)
    {
        foreach (var layerData in roomData.Layers)
        {
            if (layerData.LayerType == LayerType.Tile)
            {
                var tileData = layerData.Tiles;
                var layerId = (LayerId)Enum.Parse(typeof(LayerId), tileData.LayerId); // Get Enum from string
                var tilesetType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes()).FirstOrDefault(t => t.Name == tileData.TilesetType);
                LoadTileData(room.GetTileLayout(layerId, tilesetType), tileData);
            }
            else if (layerData.LayerType == LayerType.Entity)
            {
                var entityLayout = room.GetEntityLayout();
                LoadEntityData(entityLayout, layerData.Entities);
            }
        }
    }

    private void LoadTileData(TileLayout tileLayout, TileData tileData)
    {
        if (tileData == null)
            return;

        tileLayout.CopyLayout(tileData.Layout);
    }

    private void LoadEntityData(EntityLayout entityLayout, List<EntityData> entitiesData)
    {
        entityLayout.List ??= new();
        foreach (var entityData in entitiesData)
        {
            var entityType = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(t => t.Name == entityData.EntityType);
            if (entityType == null)
                continue;

            var entityInstance = new EntityInstance(entityLayout, entityType, IntVector2Nullable.ToIntVector2(entityData.Position));
            entityLayout.List.Add(entityInstance);
            entityInstance.CustomValues = entityData.CustomValues?.ToList() ?? new();
            // If entity was saved with less custom values, and more were added to their type, add empty ones
            var totalCustomValues = CollectionManager.GetEntityFromType(entityType).CustomValueHandler?.CustomValues.Count ?? 0;
            for (var i = entityInstance.CustomValues.Count; i < totalCustomValues; i++)
                entityInstance.CustomValues.Add(0);

            if (GameManager.GameSpecificSettings.PlayerTypes.Contains(entityType))
                Stage.PlayerStartingPosition = IntVector2Nullable.ToIntVector2(entityData.Position) + entityLayout.Room.PositionInPixels;
        }
    }
}

public enum LayerType
{
    Tile,
    Entity
}