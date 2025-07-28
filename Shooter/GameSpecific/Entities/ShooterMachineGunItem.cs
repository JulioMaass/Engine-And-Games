using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using ShooterGame.GameSpecific;

namespace ShooterGame.GameSpecific.Entities;

public class ShooterMachineGunItem : Entity
{
    public ShooterMachineGunItem()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("ShooterMachineGunItem");
        AddCenteredCollisionBox(8);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);
        AddGravity();

        AddItemComponents(ResourceType.MachineGunAmmo, 20);
        AddEquipmentItemStats(EquipGroup.Weapon);
        EquipmentItemStats.EquipmentStats.ExtraAttackSpeed = 1f;
        EquipmentItemStats.EquipmentAmmo = 10;

        //MenuItem = new MenuItem(this);
        //MenuItem.Label = "Wax Ball";
    }
}