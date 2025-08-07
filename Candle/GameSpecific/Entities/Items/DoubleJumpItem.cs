using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Items;

public class DoubleJumpItem : Entity
{
    public DoubleJumpItem()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("DoubleJumpItem", 16);
        AddCenteredCollisionBox(14);
        AddEquipmentItemStats(EquipKind.Foot);
        EquipmentItemStats.Stats.DoubleJump = true;

        MenuItem = new MenuItem(this);
        MenuItem.Label = "Double Jump";
        AddCandleEquipmentItemComponents();
    }
}