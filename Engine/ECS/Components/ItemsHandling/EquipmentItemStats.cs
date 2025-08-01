using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.GlobalManagement;
using Engine.Types;
using System.Linq;

namespace Engine.ECS.Components.ItemsHandling;

public class EquipmentItemStats : Component
{
    public EquipGroup EquipGroup { get; set; }
    public Stats EquipmentStats { get; set; }
    public int EquipmentAmmo { get; set; }

    public EquipmentItemStats(Entity owner)
    {
        Owner = owner;
    }

    public int GetLevel() =>
        GlobalManager.Values.EquipmentItemLevels.Find(i => i.Type == Owner.GetType()).Level;

    public bool IsUnlocked() =>
        GetLevel() > 0;

    public bool IsEquipped()
    {
        var (group, stats) = EquipmentHolder.GetGroupAndStatsFromType(Owner.GetType());
        var player = EntityManager.GetFilteredEntitiesFrom(EntityKind.Player).FirstOrDefault();
        var equipmentGroup = player?.EquipmentHolder.EquipmentGroups
            .FirstOrDefault(e => e.Group == group);
        var selectedTypes = equipmentGroup?.Equipments
            .Select(e => e.Type)
            .ToList();
        return selectedTypes?.Contains(Owner.GetType()) == true;
    }
}

public enum EquipGroup
{
    Weapon,
    SecondaryWeapon,
    Armor,
    Foot
}
