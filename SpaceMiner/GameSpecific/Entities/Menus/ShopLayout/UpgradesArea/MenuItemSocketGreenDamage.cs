using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;

public class MenuItemSocketGreenDamage : Entity
{
    public MenuItemSocketGreenDamage()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("UpgradeGreenDamage", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.WeaponUpgrade);
        EquipmentItemStats.Stats.ExtraDamagePercentage = 1f;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreGreen, 150, 300, 500, 1000);

        MenuItem.Label = "Damage";
        AddSpaceMinerUpgradeItemComponents();
    }
}