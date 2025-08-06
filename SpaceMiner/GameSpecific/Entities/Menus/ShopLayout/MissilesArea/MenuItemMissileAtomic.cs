using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shooters;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.MissilesArea;

public class MenuItemMissileAtomic : Entity
{
    public MenuItemMissileAtomic()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("MissileAtomic", 32);
        AddEquipmentItemStats(EquipGroup.SecondaryWeapon);
        EquipmentItemStats.EquipmentStats.SecondaryShooter = typeof(ShipShooterMissileAtomic);

        AddItemComponents(ResourceType.MissileAtomic, 1);
        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreRed, 5);

        MenuItem.Label = "Atomic";
        AddSpaceMinerMissileItemComponents();
    }
}