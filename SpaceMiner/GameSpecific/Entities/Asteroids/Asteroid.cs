using Engine.ECS.Entities.EntityCreation;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class Asteroid : Entity
{
    public Asteroid()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("Asteroid");
        AddCenteredOutlinedCollisionBox();
        AddSpaceMinerEnemyComponents(10, 1);
        AddSolidBehavior();
        //SpawnManager.DespawnOnScreenExit = false;
        AddItemDropper(
            (typeof(OreGray), 1)
        );

        AddMoveSpeed(1f);
        Speed.Acceleration = 0.08f;
        Speed.MaxSpeed = 1f;
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