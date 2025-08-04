using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shooters;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.MissilesArea;

public class MenuItemMissileSpray : Entity
{
    public MenuItemMissileSpray()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("MissileSpray", 32);
        AddEquipmentItemStats(EquipGroup.SecondaryWeapon);
        EquipmentItemStats.EquipmentStats.SecondaryShooter = typeof(ShipShooterMissileSpray);

        AddItemComponents(ResourceType.MissileSpray, 5);
        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreYellow, 10);

        MenuItem.Label = "Spray";
        AddSpaceMinerMissileItemComponents();
    }
}