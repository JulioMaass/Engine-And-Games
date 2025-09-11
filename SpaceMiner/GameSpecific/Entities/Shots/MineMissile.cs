using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Direction;
using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Entities.Shared;
using Engine.Helpers;

namespace SpaceMiner.GameSpecific.Entities.Shots;

public class MineMissile : Entity
{
    public MineMissile()
    {
        EntityKind = EntityKind.None;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("Mine");
        AddCenteredCollisionBox(16);

        AddMoveDirection();
        AddMoveSpeed(2.5f);
        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(10, PiercingType.PierceNone);
        DamageDealer.AddOnHitBehavior(new BehaviorCreateBlast(typeof(ResizableBlast), EntityKind.PlayerShot, AlignmentType.Friendly, 15, 100, 32, CustomColor.PicoBlue));

        // State
        AddStateManager();
        // Auto States
        var stateDash = NewState()
            .AddStateSettingBehavior(new BehaviorMoveToCurrentDirection())
            .AddBehaviorWithConditions(new BehaviorDecelerateMomentum(40), new ConditionFrame(2, ComparisonType.Greater))
            .AddToAutomaticStatesList();
    }
}