using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.ShipArea;

public class MenuItemMagnet : Entity
{
    public MenuItemMagnet()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("ItemMagnet", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.None);
        EquipmentItemStats.Stats.ExtraItemAttractionRadius = 32;

        AddItemComponents(ResourceType.Hp, 1000000);
        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreOrange, 100, 1000, 10000, 100000, 1000000);

        MenuItem.Label = "Magnet";
        AddSpaceMinerMissileItemComponents();
    }
}