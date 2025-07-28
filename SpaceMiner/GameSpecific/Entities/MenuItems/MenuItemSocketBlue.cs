using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Entities.Shared.MenuItemTemplates;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.MenuItems;

public class MenuItemSocketBlue : Entity
{
    public MenuItemSocketBlue()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("UpgradeBlue", 16);
        AddEquipmentItemStats(EquipGroup.Weapon);
        EquipmentItemStats.EquipmentStats.ExtraAttackSpeed = 0.5f;
        EquipmentItemStats.EquipmentStats.ExtraSpeed = 0.25f;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreBlue, 5, 10, 15, 30, 50, 100, 150, 200);

        MenuItem.Label = "AtSpeed";
        AddSpaceMinerEquipmentItemComponents();
    }
}