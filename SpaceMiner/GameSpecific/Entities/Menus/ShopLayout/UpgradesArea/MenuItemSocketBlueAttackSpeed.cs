using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;

public class MenuItemSocketBlueAttackSpeed : Entity
{
    public MenuItemSocketBlueAttackSpeed()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("UpgradeBlueAttackSpeed", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.WeaponUpgrade);
        EquipmentItemStats.Stats.ExtraAttackSpeed = 0.25f;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddUpgradePrices(ResourceType.OreBlue, ResourceType.OreGray,
            (10, 100),
            (25, 250),
            (50, 500),
            (150, 1500),
            (200, 200),
            (250, 250),
            (300, 300),
            (400, 400),
            (500, 500),
            (750, 750),
            (1000, 1000),
            (1500, 1500),
            (2000, 2000),
            (2500, 2500),
            (3000, 3000),
            (5000, 5000)
            );

        MenuItem.Label = "Attack Speed";
        AddSpaceMinerWeaponUpgradeItemComponents();
    }
}