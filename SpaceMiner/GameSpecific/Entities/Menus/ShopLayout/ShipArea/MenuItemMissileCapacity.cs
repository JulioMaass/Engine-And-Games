using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using System;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.ShipArea;

public class MenuItemMissileCapacity : Entity
{
    public MenuItemMissileCapacity()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("ItemMissileCapacity", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.None);

        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
            if (resourceType.ToString().StartsWith("Missile", StringComparison.OrdinalIgnoreCase))
                AddResourceItemStats(resourceType, 1, IncreaseKind.Max);

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddUpgradePrices(ResourceType.OreYellow, ResourceType.OreGray,
            (10, 25),
            (25, 50),
            (50, 150),
            (150, 200),
            (200, 250),
            (250, 300),
            (300, 400),
            (400, 500),
            (500, 750),
            (750, 1000),
            (1000, 1500),
            (1500, 2000),
            (2000, 2500),
            (2500, 3000),
            (3000, 5000),
            (5000, 10000)
            );

        MenuItem.Label = "Missiles";
        AddSpaceMinerShipUpgradeItemComponents();
    }
}