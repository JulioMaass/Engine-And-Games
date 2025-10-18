using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;

public class MenuItemSocketGreenDamage : Entity
{
    public MenuItemSocketGreenDamage()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("UpgradeGreenDamage", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.WeaponUpgrade);
        EquipmentItemStats.Stats.ExtraDamagePercentage = 0.25f;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddUpgradePrices(ResourceType.OreGreen, ResourceType.OreGray,
            (10, 100),
            (25, 250),
            (50, 500),
            (150, 1500),
            (200, 2000),
            (250, 2500),
            (300, 3000),
            (400, 4000),
            (500, 5000),
            (750, 7500),
            (1000, 10000),
            (1500, 15000),
            (2000, 20000),
            (2500, 25000),
            (3000, 30000),
            (5000, 50000)
            );

        MenuItem.Label = "Damage";
        AddSpaceMinerWeaponUpgradeItemComponents();
    }
}