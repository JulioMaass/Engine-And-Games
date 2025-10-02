using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;

public class MenuItemSocketYellowStraightMulti : Entity
{
    public MenuItemSocketYellowStraightMulti()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("UpgradeYellowStraightMulti", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.WeaponUpgrade);
        EquipmentItemStats.Stats.ExtraSpawnPointShots = 1;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreYellow, 100, 250, 500, 1000);

        MenuItem.Label = "Straight Multi";
        AddSpaceMinerUpgradeItemComponents();
    }
}