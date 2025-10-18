using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.ShipArea;

public class MenuItemSpeed : Entity
{
    public MenuItemSpeed()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("ItemSpeed", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.None);
        EquipmentItemStats.Stats.ExtraMoveSpeed = 0.25f;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddUpgradePrices(ResourceType.OreBlue, ResourceType.OreGray,
            (10, 10),
            (50, 50),
            (250, 250),
            (500, 500),
            (1000, 1000));

        MenuItem.Label = "Speed";
        AddSpaceMinerShipUpgradeItemComponents();
    }
}