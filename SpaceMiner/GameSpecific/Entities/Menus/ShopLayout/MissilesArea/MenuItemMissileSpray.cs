using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Shooters;

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
        AddEquipmentItemStats(EquipKind.SecondaryWeapon);
        EquipmentItemStats.Stats.SecondaryShooter = typeof(ShipShooterMissileSpray);

        AddItemComponents(ResourceType.MissileSpray, 1);
        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreYellow, 5);

        MenuItem.Label = "Spray";
        AddSpaceMinerMissileItemComponents();
    }
}