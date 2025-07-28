using Candle.GameSpecific.Entities;
using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Items;

public class LanceItem : Entity
{
    public LanceItem()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("LanceItem", 16);
        AddCenteredCollisionBox(14);
        AddEquipmentItemStats(EquipGroup.Weapon);
        EquipmentItemStats.EquipmentStats.Shooter = typeof(CandleLanceShooter);

        MenuItem = new MenuItem(this);
        MenuItem.Label = "Lance";
        AddCandleEquipmentItemComponents();
    }
}