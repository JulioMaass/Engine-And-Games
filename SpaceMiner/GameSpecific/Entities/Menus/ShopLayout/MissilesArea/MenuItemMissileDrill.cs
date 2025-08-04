using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shooters;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.MissilesArea;

public class MenuItemMissileDrill : Entity
{
    public MenuItemMissileDrill()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("MissileDrill", 32);
        AddEquipmentItemStats(EquipGroup.SecondaryWeapon);
        EquipmentItemStats.EquipmentStats.SecondaryShooter = typeof(ShipShooterMissileDrill);

        AddItemComponents(ResourceType.MissileDrill, 5);
        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreGreen, 10);

        MenuItem.Label = "Drill";
        AddSpaceMinerMissileItemComponents();
    }
}