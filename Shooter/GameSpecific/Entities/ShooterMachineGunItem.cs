using Engine.ECS.Components.ItemsHandling;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

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
        AddEquipmentItemStats(EquipKind.Weapon);
        EquipmentItemStats.Stats.ExtraAttackSpeed = 1f;
    }
}