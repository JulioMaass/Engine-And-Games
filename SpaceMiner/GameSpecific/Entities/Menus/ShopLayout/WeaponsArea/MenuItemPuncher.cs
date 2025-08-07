using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Shooters;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.WeaponsArea;

public class MenuItemPuncher : Entity
{
    public MenuItemPuncher()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("WeaponPuncher", 32);
        AddEquipmentItemStats(EquipKind.Weapon);
        EquipmentItemStats.Stats.Shooter = typeof(ShipShooterPuncher);

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrice((ResourceType.OreYellow, 250));

        MenuItem.Label = "Puncher";
        AddSpaceMinerWeaponItemComponents();
    }
}