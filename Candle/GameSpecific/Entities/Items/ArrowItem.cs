using Candle.GameSpecific.Entities;
using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Items;

public class ArrowItem : Entity
{
    public ArrowItem()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("ArrowItem", 16);
        AddCenteredCollisionBox(14);
        AddEquipmentItemStats(EquipKind.Weapon);
        EquipmentItemStats.Stats.Shooter = typeof(CandleArrowShooter);

        MenuItem = new MenuItem(this);
        MenuItem.Label = "Arrow";
        AddCandleEquipmentItemComponents();
    }
}