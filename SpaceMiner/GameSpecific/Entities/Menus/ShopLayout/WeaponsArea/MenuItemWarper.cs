using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Shooters;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.WeaponsArea;

public class MenuItemWarper : Entity
{
    public MenuItemWarper()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("WeaponWarper", 32);
        AddEquipmentItemStats(EquipKind.Weapon);
        EquipmentItemStats.Stats.Shooter = typeof(ShipShooterWarper);

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrice((ResourceType.OreBlue, 250));

        MenuItem.Label = "Warper";
        AddSpaceMinerWeaponItemComponents();
    }
}