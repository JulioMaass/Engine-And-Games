using Engine.ECS.Components.ItemsHandling;
using System;
using System.Collections.Generic;

namespace Engine.Managers.GlobalManagement;

public class EquipmentData
{
    public Type Type { get; set; }
    public int Level { get; set; }
    public List<EquipmentSlot> EquipmentSlotList { get; set; } = new();

    public EquipmentData(Type type, int level)
    {
        Type = type;
        Level = level;
    }
}
