using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using SpaceMiner.GameSpecific.Shooters;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.WeaponsArea;

public class MenuItemBasicShot : Entity
{
    public MenuItemBasicShot()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("WeaponBasic", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.Weapon);
        EquipmentItemStats.Stats.Shooter = typeof(ShipShooterBasic);

        //ItemPrice = new ItemPrice(this);
        //ItemPrice.AddPrice((ResourceType.OreRed, 150));

        MenuItem.Label = "Basic";
        AddSpaceMinerWeaponItemComponents();
    }
}