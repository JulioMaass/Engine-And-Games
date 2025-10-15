using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Shooters;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.WeaponsArea;

public class MenuItemMachineGun : Entity
{
    public MenuItemMachineGun()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("WeaponMachineGun", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.Weapon);
        EquipmentItemStats.Stats.Shooter = typeof(ShipShooterMachineGun);

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddUnlockPrice((ResourceType.OreBlue, 100));

        MenuItem.Label = "Machine";
        AddSpaceMinerWeaponItemComponents();
    }
}