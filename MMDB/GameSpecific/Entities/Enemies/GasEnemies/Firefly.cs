using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace MMDB.GameSpecific.Entities.Enemies.GasEnemies;

public class Firefly : Entity
{
    public Firefly()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("Firefly", 22, 20);
        AddMmdbEnemyComponents(2, 3);
        AddCenteredCollisionBox(18, 16);
        AddSolidBehavior();

        // Enemy specific components
        AddMoveDirection();
        AddMoveSpeed(1.0f);
        Speed.Acceleration = 0.06f;
        Speed.MaxSpeed = 2f;

        // Shooter Manager
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootStill());
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        Shooter.ShotType = typeof(FireflyFire);

        // States
        AddStateManager();
        // Auto States
        var state = NewState()
            .AddBehavior(new BehaviorSetDirectionToPlayer())
            .AddBehavior(new BehaviorAccelerateToDirection())
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameLoop(10))
            .AddToAutomaticStatesList();

        // Ai Control
        AiControl.AddPermanentBehavior(new BehaviorFacePlayer());
    }
}