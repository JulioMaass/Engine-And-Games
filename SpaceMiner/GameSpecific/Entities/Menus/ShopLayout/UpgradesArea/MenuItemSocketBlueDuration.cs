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
        ItemPrice.AddPrices(ResourceType.OreBlue, 100, 150, 250, 500, 750, 1000, 1500, 2000);

        MenuItem.Label = "Shot Duration";
        AddSpaceMinerUpgradeItemComponents();
    }
}