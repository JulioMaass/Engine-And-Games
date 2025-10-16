using Candle.GameSpecific.Entities.EnemyShots;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Behaviors.State;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Candle.GameSpecific.Entities.Enemies;

public class IceCrystal : Entity
{
    public IceCrystal()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("IceCrystal");
        AddCenteredOutlinedCollisionBox();
        AddCandleEnemyComponents(3, 10);
        AddSolidBehavior();

        // Enemy specific components
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootAtPlayer());
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        Shooter.ShotType = typeof(IceCrystalShot);
        Shooter.ShotModifiers.Add(e =>
        {
            e.Speed.MoveSpeed = 2.5f;
        });
        Speed.KnockbackSpeed = 2;

        // States
        AddStateManager();
        // Override States
        var stateHurt = NewState(default, 0)
            .AddStartCondition(new ConditionKnockedBack())
            .AddKeepCondition(new ConditionFrameSmaller(20))
            .AddStateSettingBehavior(new BehaviorStop())
            .AddStateSettingBehavior(new BehaviorFacePlayer())
            .AddBehavior(new BehaviorKnockbackMovement())
            .AddBehavior(new BehaviorDecelerateSpeed(20))
            .AddToOverrideStatesList();

        // Auto States
        var idleState = NewState(new StateIdleAndFall())
            .AddStateSettingBehavior(new BehaviorStop())
            .AddPostProcessingBehaviorWithConditions(GroupedBehaviors(new BehaviorShoot(), new BehaviorResetStateFrame()),
                new ConditionFrame(new RandomInt(90, 150), ComparisonType.Equal))
            .AddBehavior(new BehaviorCircleMovement(Engine.Helpers.Axes.Y, 3, 100, 180000))
            .AddToAutomaticStatesList();
    }
}