using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.StageHandling;
using Engine.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Engine.ECS.Entities;

/// <summary>
/// Creates and manages entities in the ECS system.
/// </summary>
public static class EntityManager // TODO - DEBUG: Show total entities per frame and list of entities
{
    public static Entity PlayerEntity => GetFilteredEntitiesFrom(EntityKind.Player).FirstOrDefault();

    public static EntityList ComponentEnforcerCheckingList { get; } = new();
    private static EntityList AllEntities { get; } = new();
    private static List<EntityList> KindLists { get; } = new(); // i.e. KindLists[1] = Players, KindLists[2] = Player shots, KindLists[3] = Enemies...

    public static Entity CreateEntityAt(Type entityType, IntVector2 position) // Type as a parameter
    {
        var entity = CreateEntity(entityType);
        entity.Position.StartingPosition = position;
        entity.Position.Pixel = position;
        return entity;
    }

    public static Entity CreateEntityAt<T>(IntVector2 position) where T : Entity // Generic method
    {
        return CreateEntityAt(typeof(T), position);
    }

    public static void RunComponentEnforcerCheckingList()
    {
        foreach (var entity in ComponentEnforcerCheckingList.ToList())
        {
            entity.ComponentEnforcer.FullCheck(true);
            ComponentEnforcerCheckingList.Remove(entity);
            if (entity.EntityKind == EntityKind.None) // EntityKind.None should be temporary
                Debugger.Break();
        }
    }

    public static Entity CreateEntity(Type entityType)
    {
        var entity = (Entity)Activator.CreateInstance(entityType);
        entity?.ComponentEnforcer.FullCheck(false);

        AllEntities.Add(entity);
        AddEntityToSubList(entity);
        return entity;
    }

    public static Entity CreateEntityForCollection(Type entityType)
    {
        // Collection entities shouldn't be checked by the component enforcer
        var entity = (Entity)Activator.CreateInstance(entityType);
        ComponentEnforcerCheckingList.Remove(entity);
        return entity;
    }

    public static void CreateEntityInstance(EntityInstance entityInstance)
    {
        var entity = CreateEntityAt(entityInstance.EntityType, entityInstance.PositionAbsolute);
        entity.EntityInstance = entityInstance;
        entityInstance.SpawnedEntity = entity;
        entityInstance.CanSpawn = false;
        entity.SpawnManager?.SpawnBehaviors.ForEach(behavior => behavior.Action());

        ApplyCustomSpawnValues(entity);
        if (StageManager.IsTransitioning) // Avoid entities created during transition not getting a state
            entity.StateManager?.Update();
    }

    private static void ApplyCustomSpawnValues(Entity entity)
    {
        if (entity.CustomValueHandler == null)
            return;
        for (var id = 0; id < entity.EntityInstance!.CustomValues.Count; id++)
        {
            entity.CustomValueHandler.SetValue(id, entity.EntityInstance.CustomValues[id]);
            entity.CustomValueHandler.CustomValues[id].ValueSetter.Invoke();
        }

        // Fast-forward on spawn
        for (var i = 0; i < entity.FrameHandler.FastForwardFrames; i++)
        {
            entity.FrameHandler.AdvanceFrameCounter();
            entity.StateManager?.AddFrame();
            entity.StateManager?.Update();
            entity.Physics?.FreeMovement.MoveInPixelsAndFraction();
        }
    }

    public static void TriggerDeath(Entity entity)
    {
        DeleteEntity(entity);
        entity.ItemDropper?.DropItem();
        entity.DeathHandler?.RunDeathProcess();
    }

    public static void DeleteEntity(Entity entity)
    {
        if (entity == null) return;
        RemoveEntityFromLists(entity);
        entity.EntityInstance?.ResetSpawnedEntity();
    }

    public static void DeleteEntities(List<Entity> entities)
    {
        foreach (var entity in entities.ToList())
            DeleteEntity(entity);
    }

    private static void RemoveEntityFromLists(Entity entity)
    {
        AllEntities.Remove(entity);
        RemoveEntityFromSubList(entity);
    }

    public static void RemoveEntityFromSubList(Entity entity)
    {
        CheckToCreateSubList(entity.EntityKind);
        KindLists[(int)entity.EntityKind].Remove(entity);
    }

    public static void AddEntityToSubList(Entity entity)
    {
        CheckToCreateSubList(entity.EntityKind);
        KindLists[(int)entity.EntityKind].Add(entity);
    }

    private static void CheckToCreateSubList(EntityKind subListId)
    {
        while (KindLists.Count <= (int)subListId)
            KindLists.Add(new EntityList());
    }

    public static List<Entity> GetAllEntities() // TODO - ARCHITECTURE: Avoid getting all entities. Make smaller lists and call the filtered function
    {
        return AllEntities.ToList();
    }

    public static List<Entity> GetFilteredEntitiesFrom(EntityKind subListId) // TODO - BUG - MMDB: Test filtering shots when there is none
    {
        CheckToCreateSubList(subListId);
        return KindLists[(int)subListId];
    }
}
