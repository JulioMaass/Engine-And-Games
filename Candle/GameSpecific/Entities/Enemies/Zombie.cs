using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Behaviors.CollisionBox;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.PositionHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Enemies;

public class Zombie : Entity
{
    public Zombie()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("Zombie");
        AddCenteredOutlinedCollisionBox();
        AddCandleEnemyComponents(2, 10);
        AddSolidBehavior();

        // Enemy specific
        AddSpeed();
        Speed.MoveSpeed = 1.0f;
        AddGravity();
        Gravity.IsAffectedByGravity = false;

        // States
        AddStateManager();
        // Commanded states
        var stateHiding = NewState()
            .AddStateSettingBehavior(new BehaviorChangeBodyType(BodyType.Bypass))
            .AddStateSettingBehavior(new BehaviorCustom(() => Sprite.IsVisible = false));
        //            .AddBehaviorWithConditions(new BehaviorCancelCommandedState(), new ConditionPlayerXDistanceRange(0, 48));
        var stateJump = NewState()
            .AddKeepCondition(new ConditionIsOnTopOfSolid().Reversed())
            .AddStateSettingBehavior(new BehaviorFacePlayer())
            .AddStateSettingBehavior(new BehaviorCustom(() => Sprite.IsVisible = true))
            .AddStateSettingBehavior(new BehaviorCustom(() => Gravity.IsAffectedByGravity = true))
            .AddStateSettingBehavior(new BehaviorCustom(() => SolidBehavior.SolidInteractionType = SolidInteractionType.StopOnSolids))
            .AddStateSettingBehavior(new BehaviorSetYSpeed(-3f));
        // Auto States
        var stateWalk = NewState()
            .AddStateSettingBehavior(new BehaviorChangeBodyType(BodyType.Vulnerable))
            .AddBehavior(new BehaviorMoveToXFacing())
            .AddToAutomaticStatesList();
        // Initial state
        StateManager.CommandState(stateHiding);
        stateHiding.Frame = -1;

        // Ai Control
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(new ConditionState(stateHiding), new ConditionPlayerXDistanceRange(0, 64));
        AiControl.AddSingleStatePool(stateJump);
    }
}