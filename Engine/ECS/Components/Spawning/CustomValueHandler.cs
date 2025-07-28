using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using System;
using System.Collections.Generic;

namespace Engine.ECS.Components.Spawning;

public class CustomValueHandler : Component
{
    public List<CustomValue> CustomValues { get; } = new();
    public class CustomValue
    {
        public Action ValueSetter { get; set; }
        public string ValueName { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    public CustomValueHandler(Entity owner)
    {
        Owner = owner;
    }

    public void NewValueSetter(int id, string name, Action valueSetter)
    {
        CustomValues.AddAtIndex(new CustomValue(), id);
        CustomValues[id].ValueName = name;
        CustomValues[id].ValueSetter = valueSetter;
    }

    public void SetValue(int id, int value)
    {
        CustomValues[id].Value = value;
    }
}
