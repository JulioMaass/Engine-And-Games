using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Items;

public class LeechItem : Entity
{
    public LeechItem()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("LeechItem", 16);
        AddCenteredCollisionBox(14);
        AddEquipmentItemStats(EquipKind.Armor);
        EquipmentItemStats.Stats.HealOnKillMultiplier = 2;

        MenuItem = new MenuItem(this);
        MenuItem.Label = "Leech";
        AddCandleEquipmentItemComponents();
    }
}