using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Entities.Shared.MenuItemTemplates;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shooters;

namespace SpaceMiner.GameSpecific.Entities.MenuItems;

public class MenuItemMachineGun : Entity
{
    public MenuItemMachineGun()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("WeaponMachineGun", 32);
        AddEquipmentItemStats(EquipGroup.Weapon);
        EquipmentItemStats.EquipmentStats.Shooter = typeof(ShipShooterMachineGun);

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrice((ResourceType.OreBlue, 10));

        MenuItem.Label = "Machine";
        AddSpaceMinerEquipmentItemComponents();
    }
}