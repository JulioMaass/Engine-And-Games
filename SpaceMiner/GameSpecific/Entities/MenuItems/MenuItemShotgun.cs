using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Entities.Shared.MenuItemTemplates;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shooters;

namespace SpaceMiner.GameSpecific.Entities.MenuItems;

public class MenuItemShotgun : Entity
{
    public MenuItemShotgun()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("WeaponShotgun", 32);
        AddEquipmentItemStats(EquipGroup.Weapon);
        EquipmentItemStats.EquipmentStats.Shooter = typeof(ShipShooterShotgun);

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrice((ResourceType.OreYellow, 15));

        MenuItem.Label = "Shotgun";
        AddSpaceMinerEquipmentItemComponents();
    }
}