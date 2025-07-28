using Candle.GameSpecific.Entities.EnemyShots;
using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Types;

namespace Candle.GameSpecific.Entities.Enemies;

public class Death : Entity
{
    public Death()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("Death");
        AddCenteredOutlinedCollisionBox();
        AddCandleEnemyComponents(4, 10);
        AddSolidBehavior();

        // Enemy specific components
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootParabolic(e => e.Physics.ParabolicMovement.LaunchAtEntityWithEscapeSpeed(EntityManager.PlayerEntity, 6)));
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        Shooter.ShotType = typeof(DeathScythe);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 4f);
        Shooter.ShotModifiers.Add(e => e.AddGravity());
        Shooter.ShotModifiers.Add(e => e.Gravity.Force = 0.10f);
        Shooter.ShotModifiers.Add(e => e.Gravity.GravityDir = Dir.Backward);

        // States
        AddStateManager();
        // Auto States
        var idleState = NewState()
            .AddBehavior(new BehaviorFacePlayer())
            .AddBehaviorWithConditions(GroupedBehaviors(new BehaviorShoot(), new BehaviorResetFrame()),
                new ConditionFrame(new RandomInt(150, 180), ComparisonType.Equal))
            .AddToAutomaticStatesList();
    }
}