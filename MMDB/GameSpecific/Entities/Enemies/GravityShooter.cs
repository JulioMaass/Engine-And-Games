using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Types;
using MMDB.GameSpecific.Entities.EnemyShots;

namespace MMDB.GameSpecific.Entities.Enemies;

public class GravityShooter : Entity
{
    public GravityShooter()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("GravityShooter", 24, 24);
        AddMmdbEnemyComponents(8, 3);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootParabolic(e => e.Physics.ParabolicMovement.LaunchAtEntityAtTime(EntityManager.PlayerEntity, 40)));
        Shooter.RelativeSpawnPosition = IntVector2.New(0, -10);
        Shooter.ShotType = typeof(EnemyBullet);
        Shooter.ShotModifiers.Add(e => e.AddGravity());
        Shooter.ShotModifiers.Add(e => e.Gravity.Force = 0.30f);
        Shooter.ShotModifiers.Add(e => e.Gravity.GravityDir = Dir.Down);

        // States
        AddStateManager();
        // Auto States
        var idleState = NewState(new StateIdleAndFall())
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameLoop(100, 2, 30))
            .AddToAutomaticStatesList();
    }
}