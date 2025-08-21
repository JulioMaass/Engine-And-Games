using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Behaviors.Direction;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Gravity;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Types;

namespace Candle.GameSpecific.Entities.Enemies;

public class Owl : Entity
{
    public Owl()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("Owl", 35, 22, 17, 11);
        AddCenteredCollisionBox(20, 14);
        AddCandleEnemyComponents(2, 10);
        AddSolidBehavior();

        //// Enemy specific components
        AddMoveDirection();
        AddMoveSpeed(1.0f);
        Speed.Acceleration = 0.08f;
        AddGravity();
        Gravity.Force = 0.1f;
        Gravity.GravityDir = Dir.Up;

        // States
        AddStateManager();

        //// Override States
        //var stateHurt = NewState(default, 0)
        //    .AddStartCondition(new ConditionKnockedBack())
        //    .AddKeepCondition(new ConditionFrameSmaller(15))
        //    .AddStateSettingBehavior(new BehaviorStop())
        //    .AddStateSettingBehavior(new BehaviorFacePlayer())
        //    .AddBehavior(new BehaviorKnockbackMovement())
        //    .AddToOverrideStatesList();

        // Command states
        var stateDive = NewState(default, 1)
            .AddStateSettingBehavior(new BehaviorCustom(() => Physics.ParabolicMovement.LaunchAtEntityWithAngleSpeed(EntityManager.PlayerEntity, 4.5f))) // TODO: Make a Behavior for this
            .AddStateSettingBehavior(new BehaviorSwitchGravity(Switch.On))
            .AddKeepCondition(new ConditionCustom(() => Position.Pixel.Y > Position.StartingPosition.Y)); // TODO: Make a Condition for this

        // Auto States
        var stateChase = NewState(default, 0)
            .AddStateSettingBehavior(new BehaviorSwitchGravity(Switch.Off))
            .AddStateSettingBehavior(new BehaviorStop(Axes.Y))
            .AddBehavior(new BehaviorSetDirectionToPlayer())
            .AddBehavior(new BehaviorAccelerateToDirection(Axes.X, 1.5f))
            .AddToAutomaticStatesList();

        // Ai Control
        AiControl.AddPermanentBehavior(new BehaviorFacePlayer());
        AiControl.SetConditionsToTriggerDecision(
            new ConditionState(stateChase),
            new ConditionFrame(new RandomInt(150, 180), ComparisonType.Equal));
        AiControl.AddSingleStatePool(stateDive);
    }
}