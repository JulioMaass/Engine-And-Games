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
    private static List<Stats> GetAllStatsSources(Entity entity, bool getEntityStats, bool getShooterStats, bool getSecondaryShooterStats = false)
    {
        var statsList = new List<Stats>();
        if (entity.EquipmentHolder == null)
            return statsList;

        // Get all equipment stats
        if (getEntityStats)
            statsList.AddRange(entity.EquipmentHolder.CharData.GetAllItemsEquippedOnChar().Select(GetStatsFromType));
        if (getShooterStats)
            statsList.AddRange(entity.EquipmentHolder.CharData.GetAllItemsEquippedOnEquipment(EquipKind.Weapon).Select(GetStatsFromType));
        if (getSecondaryShooterStats)
            statsList.AddRange(entity.EquipmentHolder.CharData.GetAllItemsEquippedOnEquipment(EquipKind.SecondaryWeapon).Select(GetStatsFromType));

        return statsList;
    }

    private static Stats GetStatsFromType(Type itemType)
    {
        var entity = CollectionManager.GetEntityFromType(itemType);
        return entity?.EquipmentItemStats?.Stats;
    }

    public static bool CheckForUnlock(Entity entity, Func<Stats, bool> unlockCondition)
    {
        return GetAllStatsSources(entity, true, false, false)
            .Count(unlockCondition) > 0;
    }

    public static int GetAddedStats(Entity entity, Func<Stats, int?> propertySelector, bool entityStats, bool weaponStats, bool secondaryWeaponStats)
    {
        return GetAllStatsSources(entity, entityStats, weaponStats, secondaryWeaponStats).ToList()
            .Select(propertySelector)
            .Where(value => value.HasValue && value != 0)
            .Sum(value => value.Value);
    }

    public static float GetAddedFloatStats(Entity entity, Func<Stats, float?> propertySelector, bool entityStats, bool weaponStats, bool secondaryWeaponStats)
    {
        return GetAllStatsSources(entity, entityStats, weaponStats, secondaryWeaponStats).ToList()
            .Select(propertySelector)
            .Where(value => value.HasValue && value != 0)
            .Sum(value => value.Value);
    }

    public static int GetMultipliedStats(Entity entity, Func<Stats, int?> propertySelector, bool entityStats, bool weaponStats, bool secondaryWeaponStats)
    {
        return GetAllStatsSources(entity, entityStats, weaponStats, secondaryWeaponStats).ToList().Select(propertySelector)
            .Where(value => value.HasValue && value != 0)
            .Aggregate(1, (current, value) => current * value.Value);
    }
}
