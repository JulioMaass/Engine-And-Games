using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Shooters;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.MissilesArea;

public class MenuItemMissileHoming : Entity
{
    public MenuItemMissileHoming()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("MissileHoming", 32);
        Sprite.HudSprite = true;
        AddEquipmentItemStats(EquipKind.SecondaryWeapon);
        EquipmentItemStats.Stats.SecondaryShooter = typeof(ShipShooterMissileHoming);

        AddItemComponents(ResourceType.MissileHoming, 1);
        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreOrange, 10);

        MenuItem.Label = "Homing";
        AddSpaceMinerMissileItemComponents();
    }
}