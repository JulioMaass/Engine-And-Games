using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Shooters;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.MissilesArea;

public class MenuItemMissileMine : Entity
{
    public MenuItemMissileMine()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("MissileMine", 32);
        Sprite.HudSprite = true;
        AddEquipmentItemStats(EquipKind.SecondaryWeapon);
        EquipmentItemStats.Stats.SecondaryShooter = typeof(ShipShooterMissileMine);

        AddItemComponents(ResourceType.MissileMine, 1);
        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreGreen, 5);

        MenuItem.Label = "Mine";
        AddSpaceMinerMissileItemComponents();
    }
}