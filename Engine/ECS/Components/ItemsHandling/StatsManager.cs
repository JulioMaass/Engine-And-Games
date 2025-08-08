using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS.Components.ItemsHandling;

// Used to gather stats info from all sources (equipment, abilities, upgrades, etc.).
public static class StatsManager
{
    private static List<Stats> GetAllStatsSources(Entity entity)
    {
        var statsList = new List<Stats>();

        // Get all equipment stats
        if (entity.EquipmentHolder != null)
            statsList.AddRange(entity.EquipmentHolder.CharData.GetAllItemsEquipped().Select(GetStatsFromType));
        if (entity.Shooter?.EquipmentHolder != null)
            statsList.AddRange(entity.Shooter.EquipmentHolder.CharData.GetAllItemsEquipped().Select(GetStatsFromType));
        if (entity.SecondaryShooter?.EquipmentHolder != null)
            statsList.AddRange(entity.SecondaryShooter.EquipmentHolder.CharData.GetAllItemsEquipped().Select(GetStatsFromType));

        return statsList;
    }

    private static Stats GetStatsFromType(Type itemType)
    {
        var entity = CollectionManager.GetEntityFromType(itemType);
        return entity?.EquipmentItemStats?.Stats;
    }

    public static bool CheckForUnlock(Entity entity, Func<Stats, bool> unlockCondition)
    {
        return GetAllStatsSources(entity)
            .Count(unlockCondition) > 0;
    }

    public static Shooter GetShooterFromEntityStats(Entity entity)
    {
        var shooterList = GetAllStatsSources(entity).Select(stats => stats.Shooter);
        var shooter = Activator.CreateInstance(shooterList.FirstOrDefault(), EntityManager.PlayerEntity) as Shooter;
        return shooter;
    }

    // All sources of a stat added (int)
    public static int GetAddedStats(Entity entity, Func<Stats, int?> propertySelector)
    {
        return GetAddedStats(GetAllStatsSources(entity).ToList(), propertySelector);
    }

    public static int GetAddedStats(EquipmentHolder equipmentHolder, Func<Stats, int?> propertySelector)
    {
        return GetAddedStats(equipmentHolder.CharData.GetAllItemsEquipped().Select(GetStatsFromType).ToList(), propertySelector);
    }

    public static int GetAddedStats(List<Stats> statsList, Func<Stats, int?> propertySelector)
    {
        return statsList.Select(propertySelector)
            .Where(value => value.HasValue && value != 0)
            .Sum(value => value.Value);
    }

    // All sources of a stat added (float)
    public static float GetAddedFloatStats(Entity entity, Func<Stats, float?> propertySelector) // Adds all sources of a stat
    {
        return GetAddedFloatStats(GetAllStatsSources(entity).ToList(), propertySelector);
    }

    public static float GetAddedFloatStats(EquipmentHolder equipmentHolder, Func<Stats, float?> propertySelector) // Adds all sources of a stat
    {
        return GetAddedFloatStats(equipmentHolder.CharData.GetAllItemsEquipped().Select(GetStatsFromType).ToList(), propertySelector);
    }

    public static float GetAddedFloatStats(List<Stats> statsList, Func<Stats, float?> propertySelector)
    {
        return statsList.Select(propertySelector)
            .Where(value => value.HasValue && value != 0)
            .Sum(value => value.Value);
    }

    // All sources of a stat multiplied (int)
    public static int GetMultipliedStats(Entity entity, Func<Stats, int?> propertySelector) // Adds all sources of a stat
    {
        return GetMultipliedStats(GetAllStatsSources(entity).ToList(), propertySelector);
    }

    public static int GetMultipliedStats(EquipmentHolder equipmentHolder, Func<Stats, int?> propertySelector) // Adds all sources of a stat
    {
        return GetMultipliedStats(equipmentHolder.CharData.GetAllItemsEquipped().Select(GetStatsFromType).ToList(), propertySelector);
    }

    public static int GetMultipliedStats(List<Stats> statsList, Func<Stats, int?> propertySelector) // Multiplies all sources of a stat
    {
        return statsList.Select(propertySelector)
            .Where(value => value.HasValue && value != 0)
            .Aggregate(1, (current, value) => current * value.Value);
    }
}
