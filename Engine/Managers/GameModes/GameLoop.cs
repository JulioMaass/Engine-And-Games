using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers.GameModes;

public abstract class GameLoop
{
    public virtual void GameSpecificSetup() { }
    public abstract void Update();

    protected void UpdateAllEntities()
    {
        var totalKinds = Enum.GetValues(typeof(EntityKind)).Length;
        for (var i = 0; i < totalKinds; i++)
        {
            UpdateEntitiesOfKind((EntityKind)i);
            UpdateOutOfOrderEntitiesUpTo((EntityKind)i);
        }
        EntityManager.RemoveEntitiesMarkedForDeletionFromLists();
    }

    private void UpdateEntitiesOfKind(EntityKind entityKind)
    {
        var kindList = EntityManager.GetFilteredEntitiesFrom(entityKind)
            .ToList();

        foreach (var entity in kindList)
            entity.Update();
    }

    // Ensures all entities are updated once before the end of the frame they are created
    private void UpdateOutOfOrderEntitiesUpTo(EntityKind entityKind)
    {
        for (var kindIndex = 0; kindIndex <= (int)entityKind; kindIndex++)
        {
            List<Entity> unupdatedEntities;
            while ((unupdatedEntities = EntityManager.GetFilteredEntitiesFrom((EntityKind)kindIndex)
                       .Where(entity => !entity.Updated)
                       .ToList()).Count > 0)
                unupdatedEntities.ForEach(entity => entity.Update());
        }
    }
}
