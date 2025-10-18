using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;

public class MenuItemSocketGreenSize : Entity
{
    public MenuItemSocketGreenSize()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("UpgradeGreenSize", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.WeaponUpgrade);
        EquipmentItemStats.Stats.ExtraSize = 1;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddUpgradePrices(ResourceType.OreGreen, ResourceType.OreGray,
            (50, 500),
            (150, 1500),
            (200, 2000),
            (300, 3000),
            (400, 4000),
            (500, 5000),
            (750, 7500),
            (1000, 10000)
            );

        MenuItem.Label = "Size";
        AddSpaceMinerWeaponUpgradeItemComponents();
    }
}