using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.ShipArea;

public class MenuItemMagnet : Entity
{
    public MenuItemMagnet()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("ItemMagnet", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.None);
        EquipmentItemStats.Stats.ExtraItemAttractionRadius = 16;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddUpgradePrices(ResourceType.OreOrange, ResourceType.OreGray,
            (50, 100),
            (100, 200),
            (250, 500),
            (500, 1000),
            (1000, 2000));

        MenuItem.Label = "Magnet";
        AddSpaceMinerShipUpgradeItemComponents();
    }
}