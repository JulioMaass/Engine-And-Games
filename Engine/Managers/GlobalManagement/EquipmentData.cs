using Engine.ECS.Components.ItemsHandling;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public EquipmentSlot GetEquipmentSlot(EquipKind equipKind, SlotType slotType)
    {
        var slot = EquipmentSlotList.FirstOrDefault(slot => slot.EquipKind == equipKind && slot.SlotType == slotType);
        if (slot != null)
            return slot;
        slot = new EquipmentSlot { EquipKind = equipKind, SlotType = slotType };
        EquipmentSlotList.Add(slot);
        return slot;
    }
}
