using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;

public class MenuItemSocketGreen : Entity
{
    public MenuItemSocketGreen()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("UpgradeGreen", 16);
        AddEquipmentItemStats(EquipGroup.Weapon);
        EquipmentItemStats.EquipmentStats.ExtraDamagePercentage = 1f;
        EquipmentItemStats.EquipmentStats.ExtraSize = 1;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreGreen, 150, 300, 500, 1000);

        MenuItem.Label = "Power";
        AddSpaceMinerEquipmentItemComponents();
    }
}