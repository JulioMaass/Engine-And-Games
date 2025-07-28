using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.MenuItems;

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
        ItemPrice.AddPrices(ResourceType.OreGreen, 30, 50, 150, 400);

        MenuItem.Label = "Power";
        AddSpaceMinerEquipmentItemComponents();
    }
}