using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shooters;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.WeaponsArea;

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