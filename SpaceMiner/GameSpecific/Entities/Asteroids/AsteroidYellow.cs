using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidYellow : Entity
{
    public AsteroidYellow()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("AsteroidYellow", 32);
        AddSpriteVariation(4, 1);
        AddCenteredCollisionBox(16);
        AddSpaceMinerEnemyComponents(10, 1);
        AddSolidBehavior();
        //SpawnManager.DespawnOnScreenExit = false;
        AddItemDropper(
            (typeof(OreYellow), 1)
        );

        AddMoveSpeed(1f);
        Speed.Acceleration = 0.08f;
        Speed.MaxSpeed = 2f;
        AddMoveDirection();
        //AddDeathHandler(new BehaviorAddScore(1));

        //// Shooter Manager
        //Shooter = new Shooter(this);
        //Shooter.AddShootAction(() => Shooter.ShootAtPlayer());
        //Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        //Shooter.ShotType = typeof(ShooterEnemyShot);
        //Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 2f);

        // States
        AddStateManager();
        // Auto States
        var state = NewState()
            .AddToAutomaticStatesList();

        var deathBehavior = new BehaviorCustom(
            () =>
            {
                var splitRotation = GetRandom.UnseededInt(360000);
                for (var i = 0; i < 4; i++)
                {
                    var angle = GetRandom.UnseededInt(90000) + i * 90000 + splitRotation;
                    var asteroid = EntityManager.CreateEntityAt(typeof(AsteroidShard), Position.Pixel);
                    asteroid.AddMoveDirection(angle);
                    asteroid.Speed.SetMoveSpeedToCurrentDirection();
                }
            }
        );

        AddDeathHandler(deathBehavior);
    }
}