using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.ShipArea;

public class MenuItemDefense : Entity
{
    public MenuItemDefense()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("ItemDefense", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.None);
        EquipmentItemStats.Stats.DefensePercentage = 0.1f;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddUpgradePrices(ResourceType.OreGreen, ResourceType.OreGray,
            (100, 1000),
            (1000, 10000),
            (10000, 100000),
            (100000, 1000000));


        MenuItem.Label = "Defense";
        AddSpaceMinerShipUpgradeItemComponents();
    }
}