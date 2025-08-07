using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.GlobalManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS.Components.ItemsHandling;

public class EquipmentHolder : Component // TODO: This is entity bound, so char loses abilities on death (change?). If so, make it a global entity (like PermanentItemHolder).
{
    public List<EquipmentSlot> EquipmentSlotList { get; } = new();

    public void AddEquipmentSlot(EquipKind kind, SlotType slotType)
    {
        EquipmentSlotList.Add(new EquipmentSlot { EquipKind = kind, SlotType = slotType });
    }

    public EquipmentHolder(Entity owner, bool equipOnRespawn)
    {
        Owner = owner;
        if (!equipOnRespawn)
            return;
        foreach (var equipmentType in GlobalManager.Values.EquippedItems.ToList().Where(equipmentType => equipmentType != null))
            TryToEquipItem(equipmentType);
    }

    public void TryToEquipItem(Type itemType)
    {
        // Find slot
        var slot = GetEquipmentSlotForType(itemType);
        if (slot == null)
            return;

        // Equip item
        if (slot.SlotType == SlotType.Switch)
            slot.EquipmentList.Clear();
        slot.EquipmentList.Add(itemType);
        GlobalManager.Values.UpdateEquippedItems();

        // Set shooter stats
        var itemEntity = CollectionManager.GetEntityFromType(itemType);
        if (itemEntity.EquipmentItemStats.Stats.Shooter != null)
            Owner.Shooter = Activator.CreateInstance(itemEntity.EquipmentItemStats.Stats.Shooter, Owner) as Shooter;
        if (itemEntity.EquipmentItemStats.Stats.SecondaryShooter != null)
            Owner.SecondaryShooter = Activator.CreateInstance(itemEntity.EquipmentItemStats.Stats.SecondaryShooter, Owner) as Shooter;
    }

    private EquipmentSlot GetEquipmentSlotForType(Type itemType)
    {
        var group = GetEquipKindFromType(itemType);
        return EquipmentSlotList.FirstOrDefault(g => g.EquipKind == group);
    }

    public int GetEquipmentCount(Type itemType)
    {
        var slot = GetEquipmentSlotForType(itemType);
        return slot == null ? 0
            : slot.EquipmentList.Count(t => t == itemType);
    }

    public bool IsItemEquipped(Type itemType)
    {
        return GetEquipmentCount(itemType) > 0;
    }

    public static EquipKind GetEquipKindFromType(Type itemType)
    {
        var entity = CollectionManager.GetEntityFromType(itemType);
        return entity?.EquipmentItemStats?.EquipKind ?? EquipKind.None;
    }

    public List<Type> GetAllItemsEquipped()
    {
        return EquipmentSlotList.SelectMany(slot => slot.EquipmentList).ToList();
    }
}
