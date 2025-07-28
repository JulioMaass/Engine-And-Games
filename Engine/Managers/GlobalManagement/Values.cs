using Engine.ECS.Entities;
using Engine.Types;
using System;
using System.Collections.Generic;

namespace Engine.Managers.GlobalManagement;

public abstract class Values
{
    public int Timer { get; private set; }

    // Equipment
    public List<(Type Type, int Level)> EquipmentItemLevels { get; private set; } = new();
    public List<Type> EquippedItems { get; } = new();

    // Resources
    public Resources Resources { get; } = new();

    protected Values()
    {
        Initialize();
    }

    private void Initialize() // Load from save file
    {
        CustomInitialize();
    }

    public void Update()
    {
        Timer++;
        CustomUpdate();
    }

    protected abstract void CustomInitialize();
    protected abstract void CustomUpdate();

    public void UpdateEquippedItems()
    {
        EquippedItems.Clear();
        foreach (var equipmentGroup in EntityManager.PlayerEntity.EquipmentHolder.EquipmentGroups)
            foreach (var equipment in equipmentGroup.Equipments)
                if (equipment.Stats != null)
                    EquippedItems.Add(equipment.Type);
    }

    public void GetEquipmentItem(Type itemType)
    {
        AddEquipmentLevel(itemType);
    }

    private void AddEquipmentLevel(Type itemType)
    {
        if (!EquipmentItemLevels.Exists(i => i.Type == itemType))
            EquipmentItemLevels.Add((itemType, 0));

        var index = EquipmentItemLevels.FindIndex(i => i.Type == itemType);
        var item = EquipmentItemLevels[index];
        EquipmentItemLevels[index] = (item.Type, item.Level + 1);
    }
}
