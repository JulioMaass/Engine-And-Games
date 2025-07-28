using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Entities.Shared.MenuItemTemplates;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.MenuItems;

public class MenuItemSocketRed : Entity
{
    public MenuItemSocketRed()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("UpgradeRed", 16);
        AddEquipmentItemStats(EquipGroup.Weapon);
        EquipmentItemStats.EquipmentStats.AddedBlastLevel = 1;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreRed, 25, 75, 150, 300);

        MenuItem.Label = "Blast";
        AddSpaceMinerEquipmentItemComponents();
    }
}