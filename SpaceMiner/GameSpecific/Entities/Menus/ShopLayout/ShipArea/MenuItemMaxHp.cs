using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.ShipArea;

public class MenuItemMaxHp : Entity
{
    public MenuItemMaxHp()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("ItemHp", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.None);

        AddItemComponents(ResourceType.Hp, 50, IncreaseKind.CurrentAndMax);
        ItemPrice = new ItemPrice(this);
        ItemPrice.AddUpgradePrices(ResourceType.OreGreen, ResourceType.OreGray,
            (50, 100),
            (100, 250),
            (250, 500),
            (500, 1000),
            (1000, 2500));

        MenuItem.Label = "Max Hp";
        AddSpaceMinerShipUpgradeItemComponents();
    }
}