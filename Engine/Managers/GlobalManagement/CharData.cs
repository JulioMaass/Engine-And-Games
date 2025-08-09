using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers.GlobalManagement;

public class CharData
{
    // Equipment
    public List<EquipmentData> Equipment { get; set; } = new();
    public List<EquipmentSlot> EquipmentSlotList { get; } = new();
    public bool EquipOnRespawn { get; set; }
    // Resources
    public Resources Resources { get; set; } = new();
    // Owner
    private Entity Char => EntityManager.PlayerEntity;

    public void AddEquipmentLevel(Type itemType)
    {
        if (!Equipment.Exists(i => i.Type == itemType))
            Equipment.Add(new EquipmentData(itemType, 0));

        Equipment.FirstOrDefault(i => i.Type == itemType)!.Level += 1;
    }

    public int GetEquipmentCount(Type itemType)
    {
        var slot = GetEquipmentSlotForType(itemType);
        return slot == null ? 0
            : slot.EquipmentList.Count(t => t.Type == itemType);
    }

    public bool IsItemEquipped(Type itemType)
    {
        return GetEquipmentCount(itemType) > 0;
    }

    public void AddEquipmentSlot(EquipKind kind, SlotType slotType)
    {
        EquipmentSlotList.Add(new EquipmentSlot { EquipKind = kind, SlotType = slotType });
    }

    //public EquipmentHolder(Entity owner, bool equipOnRespawn)
    //{
    //    Owner = owner;
    //    if (!equipOnRespawn)
    //        return;
    //    foreach (var equipmentType in GlobalManager.Values.MainCharData.EquippedItems.ToList().Where(equipmentType => equipmentType != null))
    //        TryToEquipItem(equipmentType);
    //}

    public void TryToEquipItem(Type itemType)
    {
        // Find item in the owned ones
        var equipmentData = Equipment.FirstOrDefault(e => e.Type == itemType);
        if (equipmentData == null)
            return;

        // Find slot
        var slot = GetEquipmentSlotForType(itemType);
        if (slot == null)
            return;

        // Equip item on slot
        if (slot.SlotType == SlotType.Switch)
            slot.EquipmentList.Clear();
        slot.EquipmentList.Add(equipmentData);

        // Set shooter stats
        var itemEntity = CollectionManager.GetEntityFromType(itemType);
        if (itemEntity.EquipmentItemStats.Stats.Shooter != null)
            Char.Shooter = Activator.CreateInstance(itemEntity.EquipmentItemStats.Stats.Shooter, Char) as Shooter;
        if (itemEntity.EquipmentItemStats.Stats.SecondaryShooter != null)
            Char.SecondaryShooter = Activator.CreateInstance(itemEntity.EquipmentItemStats.Stats.SecondaryShooter, Char) as Shooter;
    }

    private EquipmentSlot GetEquipmentSlotForType(Type itemType)
    {
        var group = GetEquipKindFromType(itemType);
        return EquipmentSlotList.FirstOrDefault(s => s.EquipKind == group);
    }

    public static EquipKind GetEquipKindFromType(Type itemType)
    {
        var entity = CollectionManager.GetEntityFromType(itemType);
        return entity?.EquipmentItemStats?.EquipKind ?? EquipKind.None;
    }

    public List<Type> GetAllItemsEquipped()
    {
        return (from slot in EquipmentSlotList from equipment in slot.EquipmentList select equipment.Type).ToList();
    }
}
