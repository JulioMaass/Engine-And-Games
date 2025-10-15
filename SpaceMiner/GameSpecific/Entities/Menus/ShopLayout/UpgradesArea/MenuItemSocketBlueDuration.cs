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
            (50, 50),
            (75, 75),
            (100, 100),
            (150, 150),
            (200, 200),
            (250, 250),
            (350, 350),
            (500, 500),
            (750, 750),
            (1000, 1000),
            (1500, 1500),
            (2000, 2000)
            );

        MenuItem.Label = "Shot Duration";
        AddSpaceMinerWeaponUpgradeItemComponents();
    }
}