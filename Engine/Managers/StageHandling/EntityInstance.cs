using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.Graphics;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.Managers.StageHandling;

public class EntityInstance
{
    public EntityLayout EntityLayout { get; }
    public Room Room => EntityLayout.Room;
    public Type EntityType { get; }
    public IntVector2 PositionAbsolute => PositionOnRoom + Room.PositionInPixels;
    public IntVector2 PositionOnRoom { get; set; }
    public Entity SpawnedEntity { get; set; }
    public bool CanSpawn { get; set; } = true;
    public IntRectangle CollisionBox { get; private set; }
    public List<int> CustomValues { get; set; }

    public EntityInstance(EntityLayout entityLayout, Type entityType, IntVector2 positionOnRoom)
    {
        EntityLayout = entityLayout;
        EntityType = entityType;
        PositionOnRoom = positionOnRoom;

        var entity = CollectionManager.GetEntityFromType(EntityType);
        CustomValues = new();
        for (var i = 0; i < entity.CustomValueHandler?.CustomValues.Count; i++)
            CustomValues.Add(0);
    }

    public IntRectangle GetCollisionBox()
    {
        var sprite = CollectionManager.GetEntityFromType(EntityType).Sprite;
        var collisionBoxPosition = PositionAbsolute - sprite.Origin;
        return new IntRectangle(collisionBoxPosition, sprite.Size);
    }

    public void ResetSpawnedEntity() =>
        SpawnedEntity = null;

    public bool IsOnDrawScreen()
    {
        var entitySprite = CollectionManager.GetEntityFromType(EntityType).Sprite;
        var previewRectangle = new IntRectangle(PositionAbsolute - entitySprite.Origin, entitySprite.Size);
        return Camera.DrawScreenLimits.Overlaps(previewRectangle);
    }

    public void DrawCustomValues(int valueToSkip)
    {
        if (!IsOnDrawScreen())
            return;
        var entity = CollectionManager.GetEntityFromType(EntityType);
        if (entity.CustomValueHandler == null)
            return;
        for (var i = 0; i < entity.CustomValueHandler.CustomValues.Count; i++)
        {
            if (i == valueToSkip)
                continue;
            var customValue = entity.CustomValueHandler.CustomValues[i];
            if (string.IsNullOrEmpty(customValue?.ValueName))
                continue;
            var position = PositionAbsolute + entity.Sprite.Size - entity.Sprite.Origin;
            StringDrawer.DrawStringOutlined(StringDrawer.TinyUnicodeFont, customValue.ValueName + ": " + CustomValues[i], position + (0, i * 8), Color.White);
        }
    }
}