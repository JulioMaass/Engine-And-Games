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
    public List<EquipmentData> Equipment { get; } = new();
    public List<EquipmentSlot> EquipmentSlotList { get; } = new();
    public bool EquipOnRespawn { get; set; }
    // Resources
    public Resources Resources { get; set; } = new();
    // Owner
    private Entity Char => EntityManager.PlayerEntity;
    // Cached list char's equipment
    public List<Type> CachedAllItemsEquippedOnChar { get; } = new();
    public List<Type> CachedAllItemsEquippedOnWeapon { get; } = new();
    public List<Type> CachedAllItemsEquippedOnSecondaryWeapon { get; } = new();

    public void Update()
    {
        UpdateCachedLists();
    }

    private void UpdateCachedLists()
    {
        CachedAllItemsEquippedOnChar.Clear();
        CachedAllItemsEquippedOnChar.AddRange(GetAllItemsEquippedOnChar());
        CachedAllItemsEquippedOnWeapon.Clear();
        CachedAllItemsEquippedOnWeapon.AddRange(GetAllItemsEquippedOnEquipment(EquipKind.Weapon));
        CachedAllItemsEquippedOnSecondaryWeapon.Clear();
        CachedAllItemsEquippedOnSecondaryWeapon.AddRange(GetAllItemsEquippedOnEquipment(EquipKind.SecondaryWeapon));
    }

    public void AddSwitchEquipment(Type itemType, int startLevel, int stackLevel)
    {
        if (!Equipment.Exists(i => i.Type == itemType))
        {
            Equipment.Add(new EquipmentData(itemType, startLevel));
            return;
        }
        Equipment.FirstOrDefault(i => i.Type == itemType)!.Level += stackLevel;
    }

    public void AddStackEquipment(Type itemType)
    {
        Equipment.Add(new EquipmentData(itemType, 1));
    }

    public int GetEquipmentCount(Type itemType)
    {
        var slot = GetEquipmentSlotForType(itemType);
        return slot == null ? 0
            : slot.Equipment.Count(t => t.Type == itemType);
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
            slot.Equipment.Clear();
        slot.Equipment.Add(equipmentData);

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

    private List<Type> GetAllItemsEquippedOnChar()
    {
        return (from slot in EquipmentSlotList
                from equipment in slot.Equipment
                select equipment.Type).ToList();
    }

    private List<Type> GetAllItemsEquippedOnEquipment(EquipKind equipKind)
    {
        return (from slot in EquipmentSlotList
                where slot.EquipKind == equipKind
                from equipment in slot.Equipment
                from nestedSlot in equipment.EquipmentSlotList
                from nestedEquipment in nestedSlot.Equipment
                select nestedEquipment.Type).ToList();
    }

    public int GetAmount(Type equipmentType)
    {
        return Equipment.Count(e => e.Type == equipmentType);
    }

    public Type GetCurrentEquippedTypeOnSlot(EquipKind equipKind)
    {
        return EquipmentSlotList.FirstOrDefault(s => s.EquipKind == equipKind)?.Equipment
            .FirstOrDefault()?.Type;
    }

    public int GetAmountOfUpgradesOnWeapon(Type upgradeType, Type weaponType = null)
    {
        weaponType ??= GetCurrentEquippedTypeOnSlot(EquipKind.Weapon);
        return weaponType == null ? 0 : GetAllUpgradesOnWeapon(weaponType).Count(t => t == upgradeType);
    }

    public List<Type> GetAllUpgradesOnWeapon(Type weaponType = null)
    {
        weaponType ??= GetCurrentEquippedTypeOnSlot(EquipKind.Weapon);
        var upgradeList = Equipment.FirstOrDefault(e => e.Type == weaponType)
            ?.GetEquipmentSlot(EquipKind.WeaponUpgrade, SlotType.Stack)
            .Equipment.Select(e => e.Type).ToList();
        return upgradeList ?? new List<Type>();
    }

    public void AddUpgradeToWeapon(Type upgradeType, Type weaponType = null)
    {
        weaponType ??= GetCurrentEquippedTypeOnSlot(EquipKind.Weapon);
        if (weaponType == null)
            return;
        Equipment.FirstOrDefault(e => e.Type == weaponType)
            .GetEquipmentSlot(EquipKind.WeaponUpgrade, SlotType.Stack)
            .AddEquipment(upgradeType, 1);
    }
}
