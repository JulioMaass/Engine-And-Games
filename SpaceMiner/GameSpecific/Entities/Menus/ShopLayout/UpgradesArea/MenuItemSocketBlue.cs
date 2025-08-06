using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;

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
        ItemPrice.AddPrices(ResourceType.OreBlue, 100, 150, 250, 500, 750, 1000, 1500, 2000);

        MenuItem.Label = "AtSpeed";
        AddSpaceMinerEquipmentItemComponents();
    }
}