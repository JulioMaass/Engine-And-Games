using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;

public class MenuItemSocketYellow : Entity
{
    public MenuItemSocketYellow()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("UpgradeYellow", 16);
        AddEquipmentItemStats(EquipGroup.Weapon);
        EquipmentItemStats.EquipmentStats.ExtraShots = 1;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreYellow, 200, 500, 1500, 3000);

        MenuItem.Label = "Multi";
        AddSpaceMinerEquipmentItemComponents();
    }
}