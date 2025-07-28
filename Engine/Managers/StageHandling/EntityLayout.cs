using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Managers.Graphics;
using Engine.Types;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers.StageHandling;

public class EntityLayout : Layer
{
    public List<EntityInstance> List { get; set; } = new();
    public Room Room { get; }

    public EntityLayout(Room room)
    {
        Room = room;
    }

    public EntityInstance GetEntityInstanceAt(IntVector2 positionAbsolute)
    {
        return List.FirstOrDefault(entityInstance => positionAbsolute == entityInstance.PositionAbsolute);
    }

    public void DrawEntityLayout()
    {
        foreach (var entityInstance in List.Where(entityInstance => entityInstance.IsOnDrawScreen()))
            CollectionManager.DrawEntityPreview(entityInstance.EntityType, entityInstance.PositionAbsolute, CustomColor.TransparentWhite);
    }

    public void DrawEntityLayoutCollisionBox()
    {
        foreach (var entityInstance in List)
        {
            var collisionBox = entityInstance.GetCollisionBox();
            Drawer.DrawRectangleOutline(collisionBox.Position, collisionBox.Size, CustomColor.TransparentWhite);
        }
    }
}
