using Engine.ECS.Components.ControlHandling.Behaviors.DeathAndDestroy;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using MMDB.GameSpecific.Entities.Explosions;

namespace MMDB.GameSpecific.Entities.EnemyShots;

public class CannonBomb : Entity
{
    public CannonBomb()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("CannonBomb", 16, 16);
        AddMmdbEnemyShotComponents(3);
        AddDamageTaker(1);
        AddSolidBehavior();
        AddMoveDirection();
        AddGravity(0.075f);
        AddDeathHandler(new BehaviorShoot());

        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootStill());
        Shooter.ShotType = typeof(ExplosionBig);
        Shooter.BaseDamage = 3;
        Shooter.EntityKind = EntityKind.EnemyShot;

        // States
        AddStateManager();
        var state = NewState()
            .AddBehaviorWithConditions(new BehaviorDestroy(), new ConditionFacingCollides());
        StateManager.AutomaticStatesList.Add(state);
    }
}