using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Items;

public class ArmorItem : Entity
{
    public ArmorItem()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("ArmorItem", 16);
        AddCenteredCollisionBox(14);
        AddEquipmentItemStats(EquipKind.Armor);
        EquipmentItemStats.Stats.DefensePercentage = 1;

        MenuItem = new MenuItem(this);
        MenuItem.Label = "Armor";
        AddCandleEquipmentItemComponents();
    }
}