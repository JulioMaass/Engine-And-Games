using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Shooters;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.WeaponsArea;

public class MenuItemBlaster : Entity
{
    public MenuItemBlaster()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("WeaponBlaster", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.Weapon);
        EquipmentItemStats.Stats.Shooter = typeof(ShipShooterBlaster);
        EquipmentItemStats.Stats.AddedBlastLevel = 1; // Turns on the blast

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddUnlockPrice((ResourceType.OreRed, 150));

        MenuItem.Label = "Blaster";
        AddSpaceMinerWeaponItemComponents();
    }
}