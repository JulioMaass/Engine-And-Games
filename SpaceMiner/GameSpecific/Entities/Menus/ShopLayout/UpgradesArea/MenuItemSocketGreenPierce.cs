using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;

public class MenuItemSocketGreenPierce : Entity
{
    public MenuItemSocketGreenPierce()
    {
        EntityKind = EntityKind.Menu;
        AddBasicComponents();
        MenuItem = new(this);

        // Basic, Sprite, EntityKind
        AddSpriteCenteredOrigin("UpgradeGreenPierce", 32);
        Sprite.HudSprite = true;
        AddFrameSprite("MenuSocketNineSlice", 2, 2);
        AddEquipmentItemStats(EquipKind.WeaponUpgrade);
        EquipmentItemStats.Stats.ExtraPierceAmount = 1;

        ItemPrice = new ItemPrice(this);
        ItemPrice.AddPrices(ResourceType.OreGreen, 150, 300, 500, 1000);

        MenuItem.Label = "Pierce";
        AddSpaceMinerUpgradeItemComponents();
    }
}