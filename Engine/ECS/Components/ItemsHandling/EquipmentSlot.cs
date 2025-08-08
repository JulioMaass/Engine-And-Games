using System;
using System.Collections.Generic;

namespace Engine.ECS.Components.ItemsHandling;

public class EquipmentSlot : Component
{
    public EquipKind EquipKind { get; set; }
    public SlotType SlotType { get; set; }
    public List<Type> EquipmentList { get; set; } = new();
}

public enum SlotType
{
    None,
    Switch, // can hold one item at a time, equipping switches items
    Stack, // can hold multiple items, equipping stacks items
}