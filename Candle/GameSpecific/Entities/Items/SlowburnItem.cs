using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Items;

public class SlowburnItem : Entity
{
    public SlowburnItem()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("SlowburnItem", 16);
        AddCenteredCollisionBox(14);
        AddEquipmentItemStats(EquipKind.Armor);
        EquipmentItemStats.Stats.BurningRateMultiplier = 2;

        MenuItem = new MenuItem(this);
        MenuItem.Label = "Slowburn";
        AddCandleEquipmentItemComponents();
    }
}