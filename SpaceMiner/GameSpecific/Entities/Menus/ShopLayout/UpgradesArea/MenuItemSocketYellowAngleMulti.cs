using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;

public class MenuItemSocketYellowAngleMulti : Entity
{
    public MenuItemSocketYellowAngleMulti()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("UpgradeYellowAngleMulti", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.WeaponUpgrade);
        EquipmentItemStats.Stats.ExtraAngleShots = 1;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddUpgradePrices(ResourceType.OreYellow, ResourceType.OreGray,
            (100, 1000),
            (250, 2500),
            (500, 5000),
            (1000, 10000)
            );

        MenuItem.Label = "Angle Multi";
        AddSpaceMinerWeaponUpgradeItemComponents();
    }
}