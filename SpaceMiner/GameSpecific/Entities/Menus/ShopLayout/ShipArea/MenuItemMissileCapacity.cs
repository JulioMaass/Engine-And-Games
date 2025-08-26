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
        AddEquipmentItemStats(EquipKind.None);

        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
            if (resourceType.ToString().StartsWith("Missile", StringComparison.OrdinalIgnoreCase))
                AddResourceItemStats(resourceType, 10, IncreaseKind.Max);

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreYellow, 100, 1000, 10000, 100000, 1000000);

        MenuItem.Label = "Missiles";
        AddSpaceMinerMissileItemComponents();
    }
}