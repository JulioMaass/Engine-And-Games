using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Entities.Shared.MenuItemTemplates;
using Engine.Types;
using SpaceMiner.GameSpecific;

namespace SpaceMiner.GameSpecific.Entities.MenuItems;

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
        ItemPrice.AddPrices(ResourceType.OreYellow, 20, 50, 150, 300);

        MenuItem.Label = "Multi";
        AddSpaceMinerEquipmentItemComponents();
    }
}