using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shooters;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.WeaponsArea;

public class MenuItemSlugger : Entity
{
    public MenuItemSlugger()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("WeaponSlugger", 32);
        AddEquipmentItemStats(EquipKind.Weapon);
        EquipmentItemStats.Stats.Shooter = typeof(ShipShooterSlugger);

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrice((ResourceType.OreGreen, 100));

        MenuItem.Label = "Slugger";
        AddSpaceMinerWeaponItemComponents();
    }
}