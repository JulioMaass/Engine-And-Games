using Candle.GameSpecific.Entities.EnemyShots;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Teleporting;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Candle.GameSpecific.Entities.Enemies;

public class Shade : Entity
{
    public Shade()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("Shade", 48, 40, 22, 19);
        AddCenteredCollisionBox(16, 30);
        AddCandleEnemyComponents(4, 10);
        AddSolidBehavior();

        // Enemy specific components
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootStill());
        Shooter.RelativeSpawnPosition = IntVector2.New(20, 0);
        Shooter.ShotType = typeof(ShadeSlash);

        // States
        AddStateManager();
        // Command States
        var stateTeleport = NewState(default, 0)
            .AddBehaviorWithConditions(new BehaviorTeleportTo(EntityManager.PlayerEntity, (-30, 0), true),
            new ConditionFrame(0, ComparisonType.Equal))
            .AddBehavior(new BehaviorFacePlayer())
            .AddBehavior(new BehaviorCircleMovement(Engine.Helpers.Axes.Y, 5, 60, 180000))
            .AddKeepCondition(new ConditionFrame(60, ComparisonType.LessOrEqual));
        var stateAttack = NewState(default, 1)
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrame(0, ComparisonType.Equal))
            .AddBehavior(new BehaviorStop())
            .AddKeepCondition(new ConditionFrame(30, ComparisonType.LessOrEqual));

        // Auto States
        var stateIdle = NewState(default, 0)
            .AddBehavior(new BehaviorFacePlayer())
            .AddBehavior(new BehaviorCircleMovement(Engine.Helpers.Axes.Y, 5, 60, 180000))
            .AddToAutomaticStatesList();

        // Ai Control
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(
            new ConditionState(stateIdle),
            new ConditionFrame(new RandomInt(90, 120), ComparisonType.Equal));
        AiControl.AddSinglePatternPool(stateTeleport, stateAttack);
    }
}