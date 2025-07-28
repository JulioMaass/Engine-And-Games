using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Managers.GlobalManagement;
using Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS.Components.ItemsHandling;

public class EquipmentHolder : Component // TODO: This is entity bound, so char loses abilities on death (change?). If so, make it a global entity (like PermanentItemHolder).
{
    public List<EquipmentGroup> EquipmentGroups { get; } = new();

    public class EquipmentGroup
    {
        public EquipGroup Group { get; set; }
        public bool CanStack { get; set; }
        public List<Equipment> Equipments { get; set; } = new();
    }

    public class Equipment
    {
        public Type Type { get; set; }
        public Stats Stats { get; set; }
    }

    public void AddEquipmentGroup(EquipGroup group, bool canStack)
    {
        if (EquipmentGroups.Any(g => g.Group == group))
            return;
        EquipmentGroups.AddAtIndex(new EquipmentGroup { Group = group, CanStack = canStack }, (int)group);
    }

    public EquipmentHolder(Entity owner, bool equipOnRespawn)
    {
        Owner = owner;

        if (!equipOnRespawn) return;
        foreach (var equipmentType in GlobalManager.Values.EquippedItems.ToList().Where(equipmentType => equipmentType != null))
            EquipItem(equipmentType);
    }

    public void EquipItem(Type itemType)
    {
        var (group, stats) = GetGroupAndStatsFromType(itemType);
        var equipment = new Equipment { Type = itemType, Stats = stats };
        var equipmentGroup = EquipmentGroups.FirstOrDefault(g => g.Group == group);
        if (equipmentGroup == null)
            return;

        if (!equipmentGroup.CanStack)
            equipmentGroup.Equipments.Clear();
        equipmentGroup.Equipments.Add(equipment);
        GlobalManager.Values.UpdateEquippedItems();

        // Set shooter stats
        if (stats.Shooter != null)
            Owner.Shooter = Activator.CreateInstance(stats.Shooter, Owner) as Shooter;
    }

    public int GetEquipmentItemCount(Type itemType)
    {
        var (group, _) = GetGroupAndStatsFromType(itemType);
        var equipmentGroup = EquipmentGroups.FirstOrDefault(g => g.Group == group);
        if (equipmentGroup == null)
            return 0;
        return equipmentGroup.Equipments.Count(e => e.Type == itemType);
    }

    public static (EquipGroup, Stats) GetGroupAndStatsFromType(Type itemType)
    {
        var entity = CollectionManager.GetEntityFromType(itemType);
        var group = entity.EquipmentItemStats.EquipGroup;
        var stats = entity.EquipmentItemStats.EquipmentStats;
        return (group, stats);
    }

    public void GetEquipmentItem(Entity item)
    {
        var itemType = item.GetType();
        GlobalManager.Values.GetEquipmentItem(itemType);
    }
}
