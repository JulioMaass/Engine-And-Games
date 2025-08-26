using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.ShipArea;

public class MenuItemRepair : Entity
{
    public MenuItemRepair()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("ItemRepair", 32);
        AddEquipmentItemStats(EquipKind.None);

        AddItemComponents(ResourceType.Hp, 1000000);
        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreGreen, 100, 1000, 10000, 100000, 1000000);

        MenuItem.Label = "Repair";
        AddSpaceMinerMissileItemComponents();
    }
}