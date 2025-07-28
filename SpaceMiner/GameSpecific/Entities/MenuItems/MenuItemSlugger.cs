using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Entities.Shared.MenuItemTemplates;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shooters;

namespace SpaceMiner.GameSpecific.Entities.MenuItems;

public class MenuItemSlugger : Entity
{
    public MenuItemSlugger()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("WeaponSlugger", 32);
        AddEquipmentItemStats(EquipGroup.Weapon);
        EquipmentItemStats.EquipmentStats.Shooter = typeof(ShipShooterSlugger);

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrice((ResourceType.OreGreen, 25));

        MenuItem.Label = "Slugger";
        AddSpaceMinerEquipmentItemComponents();
    }
}