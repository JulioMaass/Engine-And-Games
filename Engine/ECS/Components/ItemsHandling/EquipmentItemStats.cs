using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.GlobalManagement;
using Engine.Types;

namespace Engine.ECS.Components.ItemsHandling;

public class EquipmentItemStats : Component
{
    public EquipKind EquipKind { get; set; }
    public Stats Stats { get; set; }

    public EquipmentItemStats(Entity owner)
    {
        Owner = owner;
    }

    public int GetLevel() =>
        GlobalManager.Values.MainCharData.Equipment.Find(i => i.Type == Owner.GetType()).Level;

    public bool IsUnlocked() =>
        GetLevel() > 0;
}

public enum EquipKind
{
    None,
    Weapon,
    WeaponUpgrade,
    SecondaryWeapon,
    Armor,
    Foot
}
