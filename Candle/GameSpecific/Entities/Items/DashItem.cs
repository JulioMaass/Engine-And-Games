using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Items;

public class DashItem : Entity
{
    public DashItem()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("DashItem", 16);
        AddCenteredCollisionBox(14);
        AddEquipmentItemStats(EquipGroup.Foot);
        EquipmentItemStats.EquipmentStats.Dash = true;

        MenuItem = new MenuItem(this);
        MenuItem.Label = "Dash";
        AddCandleEquipmentItemComponents();
    }
}