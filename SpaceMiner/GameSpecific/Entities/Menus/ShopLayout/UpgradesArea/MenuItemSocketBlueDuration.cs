using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;

public class MenuItemSocketBlueDuration : Entity
{
    public MenuItemSocketBlueDuration()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("UpgradeBlueDuration", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.WeaponUpgrade);
        EquipmentItemStats.Stats.ExtraDuration = 10;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddUpgradePrices(ResourceType.OreBlue, ResourceType.OreGray,
            (10, 100),
            (20, 200),
            (30, 300),
            (40, 400),
            (50, 500),
            (75, 750),
            (100, 1000),
            (150, 1500),
            (200, 2000),
            (250, 2500),
            (350, 3500),
            (500, 5000),
            (750, 7500),
            (1000, 10000),
            (1500, 15000),
            (2000, 20000)
            );

        MenuItem.Label = "Shot Duration";
        AddSpaceMinerWeaponUpgradeItemComponents();
    }
}