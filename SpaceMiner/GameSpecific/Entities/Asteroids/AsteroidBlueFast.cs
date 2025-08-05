using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidBlueFast : Entity
{
    public int Teleports { get; set; }

    public AsteroidBlueFast()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("AsteroidBlue", 32);
        AddSpriteVariation(4, 1);
        AddCenteredCollisionBox(16);
        AddSpaceMinerEnemyComponents(30, 1);
        AddSolidBehavior();
        //SpawnManager.DespawnOnScreenExit = false;
        AddItemDropper(
            (typeof(OreBlue), 1)
        );

        BloomSource = new BloomSource(this, 0.75f);

        AddRandomMoveSpeed(1.75f, 2.5f);
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
            .AddBehavior(new BehaviorWarp(this, 2))
            .AddToAutomaticStatesList();
    }
}