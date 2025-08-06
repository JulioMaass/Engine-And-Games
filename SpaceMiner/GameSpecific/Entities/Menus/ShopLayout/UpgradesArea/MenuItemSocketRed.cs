using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;

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
        ItemPrice.AddPrices(ResourceType.OreRed, 150, 250, 500, 1000);

        MenuItem.Label = "Blast";
        AddSpaceMinerEquipmentItemComponents();
    }
}