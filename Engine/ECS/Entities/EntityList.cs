using Engine.ECS.Entities.EntityCreation;
using System;
using System.Collections.Generic;

namespace Engine.ECS.Entities;

public class EntityList : List<Entity>
{
    public new void Add(Entity entity)
    {
        base.Add(entity);
#if DEBUG
        if (CollectionManager.Contains(entity))
        {
            throw new Exception("Entity is from a collection, use CreateEntity or create a copy instead.");
        }
#endif
    }
}

public static class EntityListExtensions
{
    public static EntityList ToEntityList(this List<Entity> entities)
    {
        var entityList = new EntityList();
        entityList.AddRange(entities);
        return entityList;
    }

    public static EntityList ToEntityList(this IEnumerable<Entity> entities)
    {
        var entityList = new EntityList();
        entityList.AddRange(entities);
        return entityList;
    }
}