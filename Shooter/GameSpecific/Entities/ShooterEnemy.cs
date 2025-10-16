using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.DeathAndDestroy;
using Engine.ECS.Components.ControlHandling.Behaviors.Directions;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Behaviors.State;
using Engine.ECS.Components.ControlHandling.Behaviors.Targeting;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace ShooterGame.GameSpecific.Entities;

public class ShooterEnemy : Entity
{
    public ShooterEnemy()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("ShooterEnemy");
        AddCenteredOutlinedCollisionBox();
        AddShooterEnemyComponents(5, 0);
        AddSolidBehavior();
        SpawnManager.DespawnOnScreenExit = false;
        AddItemDropper(typeof(ShooterMachineGunItem));

        Speed.Acceleration = 0.08f;
        Speed.MaxSpeed = 2f;
        AddMoveDirection();
        AddDeathHandler(new BehaviorAddScore(1));

        // Shooter Manager
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootAtPlayer());
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        Shooter.ShotType = typeof(ShooterEnemyShot);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 2f);

        // States
        AddStateManager();
        // Auto States
        var state = NewState()
            .AddBehavior(new BehaviorFacePlayer())
            .AddBehavior(new BehaviorTargetNearestEntity(AlignmentType.Friendly, EntityKind.Player))
            .AddBehavior(new BehaviorSetDirectionToTarget(MoveDirection))
            .AddBehavior(new BehaviorAccelerateToDirection(Engine.Helpers.Axes.X))
            .AddPostProcessingBehaviorWithConditions(GroupedBehaviors(new BehaviorShoot(), new BehaviorResetStateFrame()),
                new ConditionFrame(new RandomInt(50, 70), ComparisonType.Equal))
            .AddToAutomaticStatesList();
    }
}