using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.ShipArea;

public class MenuItemDash : Entity
{
    public MenuItemDash()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("ItemDash", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.None);
        EquipmentItemStats.Stats.Dash = true;
        EquipmentItemStats.Stats.ExtraDashSpeed = 2f;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddUpgradePrices(ResourceType.OreBlue, ResourceType.OreGray,
            (100, 100),
            (250, 250),
            (500, 500),
            (1000, 1000));

        MenuItem.Label = "Dash";
        AddSpaceMinerShipUpgradeItemComponents();
    }
}