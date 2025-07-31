using Engine.ECS.Entities.EntityCreation;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidShard : Entity
{
    public AsteroidShard()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("AsteroidShard");
        AddCenteredOutlinedCollisionBox();
        AddSpaceMinerEnemyComponents(25, 1);
        AddSolidBehavior();
        //SpawnManager.DespawnOnScreenExit = false;
        //AddItemDropper(
        //    (typeof(ShooterMachineGunItem), 9)
        //);

        AddRandomMoveSpeed(0.8f, 1.0f);
        Speed.Acceleration = 0.08f;
        Speed.MaxSpeed = 8f;
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
    }
}