using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;

public class MenuItemSocketBlueShotSpeed : Entity
{
    public MenuItemSocketBlueShotSpeed()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("UpgradeBlueShotSpeed", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.WeaponUpgrade);
        EquipmentItemStats.Stats.ExtraSpeed = 0.25f;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddUpgradePrices(ResourceType.OreBlue, ResourceType.OreGray,
            (25, 250),
            (50, 500),
            (100, 1000),
            (150, 1500),
            (250, 250),
            (500, 500),
            (750, 750),
            (1000, 1000),
            (1500, 1500),
            (2000, 2000)
            );

        MenuItem.Label = "Shot Speed";
        AddSpaceMinerWeaponUpgradeItemComponents();
    }
}