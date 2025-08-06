using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shooters;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.WeaponsArea;

public class MenuItemBlaster : Entity
{
    public MenuItemBlaster()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("WeaponBlaster", 32);
        AddEquipmentItemStats(EquipGroup.Weapon);
        EquipmentItemStats.EquipmentStats.Shooter = typeof(ShipShooterBlaster);
        EquipmentItemStats.EquipmentStats.AddedBlastLevel = 1; // Turns on the blast

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrice((ResourceType.OreRed, 150));

        MenuItem.Label = "Blaster";
        AddSpaceMinerEquipmentItemComponents();
    }
}