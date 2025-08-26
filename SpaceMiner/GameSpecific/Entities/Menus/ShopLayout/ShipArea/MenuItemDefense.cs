using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.ShipArea;

public class MenuItemDefense : Entity
{
    public MenuItemDefense()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("ItemDefense", 32);
        AddEquipmentItemStats(EquipKind.None);
        EquipmentItemStats.Stats.AddedDefenseRatio = 1;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreGreen, 100, 1000, 10000, 100000, 1000000);

        MenuItem.Label = "Defense";
        AddSpaceMinerMissileItemComponents();
    }
}