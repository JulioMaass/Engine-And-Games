using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using MMDB.GameSpecific.Entities.EnemyShots;
using MMDB.GameSpecific.States.Enemy;

namespace MMDB.GameSpecific.Entities.Enemies;

public class BlobBob : Entity
{
    public BlobBob()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("BlobBob", 16, 16);
        AddMmdbEnemyComponents(4, 1);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components
        Speed.SetXSpeed(3f);
        Speed.CrawlTurnDirection = -1;
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootLeftAndRight());
        Shooter.ShotType = typeof(EnemyBullet);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 3f);

        // States
        AddStateManager();
        // Auto States
        var crawlState = NewState(new StateEnemyCrawl())
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameLoop(60))
            .AddToAutomaticStatesList();
    }
}