using Candle.GameSpecific.Entities;
using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Items;

public class BigSlashItem : Entity
{
    public BigSlashItem()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("BigSlashItem", 16);
        AddCenteredCollisionBox(14);
        AddEquipmentItemStats(EquipGroup.Weapon);
        EquipmentItemStats.EquipmentStats.Shooter = typeof(CandleSlashShooter);

        MenuItem = new MenuItem(this);
        MenuItem.Label = "Sword";
        AddCandleEquipmentItemComponents();
    }
}