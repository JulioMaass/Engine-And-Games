using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Direction;
using Engine.ECS.Components.ControlHandling.Behaviors.Direction;
using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Components.ControlHandling.Behaviors.Targeting;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Entities.Shared;
using Engine.Helpers;

namespace SpaceMiner.GameSpecific.Entities.Shots;

public class HomingMissile : Entity
{
    public HomingMissile()
    {
        EntityKind = EntityKind.None;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("Missile");
        Sprite.AutoRotation = true;
        AddSolidBehavior();
        AddCenteredCollisionBox(8);

        AddMoveDirection();
        AddMoveSpeed(4.5f);
        AddTurnSpeed(5000);
        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(10, PiercingType.PierceNone);
        DamageDealer.AddOnHitBehavior(new BehaviorCreateBlast(typeof(ResizableBlast), EntityKind.PlayerShot, AlignmentType.Friendly, 15, 100, 32, CustomColor.PicoBlue));

        // State
        AddStateManager();
        var state = NewState()
            .AddBehavior(new BehaviorTargetNearestEntity(AlignmentType.Hostile, EntityKind.Enemy))
            .AddBehavior(new BehaviorTurnTowardsTarget())
            .AddBehavior(new BehaviorMoveToCurrentDirection());
        StateManager.AutomaticStatesList.Add(state);
    }
}