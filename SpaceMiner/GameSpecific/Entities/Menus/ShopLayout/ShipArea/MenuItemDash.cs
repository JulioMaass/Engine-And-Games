using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.ShipArea;

public class MenuItemDash : Entity
{
    public MenuItemDash()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("ItemDash", 32);
        AddEquipmentItemStats(EquipKind.None);
        EquipmentItemStats.Stats.Dash = true;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreBlue, 10000, 1000000);

        MenuItem.Label = "Dash";
        AddSpaceMinerMissileItemComponents();
    }
}