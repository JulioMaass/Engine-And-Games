using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS.Components.ItemsHandling;

// Component used to gather stats info from all sources (equipment, abilities, upgrades, etc.). // TODO: Automatically create when the entity has any type of stats
public class StatsManager : Component
{
    public StatsManager(Entity owner)
    {
        Owner = owner;
    }

    private List<Stats> GetAllStatsSources()
    {
        var statsList = new List<Stats>();
        if (Owner.EquipmentHolder == null)
            return statsList;

        // Get all equipment stats
        foreach (var equipmentGroup in Owner.EquipmentHolder.EquipmentGroups)
            foreach (var equipment in equipmentGroup.Equipments)
                if (equipment.Stats != null)
                    statsList.Add(equipment.Stats);

        return statsList;
    }

    public int GetAddedStats(Func<Stats, int?> propertySelector) // Adds all sources of a stat
    {
        return Owner.StatsManager.GetAllStatsSources()
            .Select(propertySelector)
            .Where(value => value.HasValue && value != 0)
            .Sum(value => value.Value);
    }

    public float GetAddedFloatStats(Func<Stats, float?> propertySelector) // Adds all sources of a stat
    {
        return Owner.StatsManager.GetAllStatsSources()
            .Select(propertySelector)
            .Where(value => value.HasValue && value != 0)
            .Sum(value => value.Value);
    }

    public int GetMultipliedStats(Func<Stats, int?> propertySelector) // Multiplies all sources of a stat
    {
        return Owner.StatsManager.GetAllStatsSources()
            .Select(propertySelector)
            .Where(value => value.HasValue && value != 0)
            .Aggregate(1, (current, value) => current * value.Value);
    }

    public bool CheckForUnlock(Func<Stats, bool> unlockCondition)
    {
        return Owner.StatsManager.GetAllStatsSources()
            .Count(unlockCondition) > 0;
    }
}
