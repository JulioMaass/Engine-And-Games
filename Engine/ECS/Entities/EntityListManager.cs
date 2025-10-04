using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Engine.ECS.Entities;

public static class EntityListManager
{
    public static EntityList SolidEntities { get; } = new();
    public static EntityList SolidTopEntities { get; } = new();

    public static void AddEntityToLists(Entity entity)
    {
        if (entity.SolidBehavior?.SolidType == SolidType.Solid)
            SolidEntities.Add(entity);
        if (entity.SolidBehavior?.SolidType == SolidType.SolidTop)
            SolidTopEntities.Add(entity);
    }

    public static void RemoveEntityFromLists(Entity entity)
    {
        SolidEntities.Remove(entity);
        SolidTopEntities.Remove(entity);
    }
}
