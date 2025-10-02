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
        ItemPrice.AddPrices(ResourceType.OreBlue, 100, 150, 250, 500, 750, 1000, 1500, 2000);

        MenuItem.Label = "Shot Speed";
        AddSpaceMinerUpgradeItemComponents();
    }
}