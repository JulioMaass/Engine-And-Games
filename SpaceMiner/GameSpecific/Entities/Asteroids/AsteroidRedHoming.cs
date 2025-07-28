using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidRedHoming : Entity
{
    public AsteroidRedHoming()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("AsteroidRed");
        AddCenteredOutlinedCollisionBox();
        AddSpaceMinerEnemyComponents(10, 1);
        AddSolidBehavior();
        //SpawnManager.DespawnOnScreenExit = false;
        AddItemDropper(
            (typeof(OreRed), 1)
        );

        AddMoveSpeed(1f);
        Speed.Acceleration = 0.08f;
        Speed.MaxSpeed = 2f;
        AddMoveDirection();
        //AddDeathHandler(new BehaviorAddScore(1));+

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

    protected override void CustomUpdate()
    {
        if (EntityManager.PlayerEntity == null)
            return;
        // Set turn speed (turns slower when further away)
        if (IntVector2.GetDistance(Position.Pixel, EntityManager.PlayerEntity.Position.Pixel) < 64 + 32)
            Speed.TurnSpeed = 1;
        if (IntVector2.GetDistance(Position.Pixel, EntityManager.PlayerEntity.Position.Pixel) < 64)
            Speed.TurnSpeed = 2;
        // Turn towards the player
        if (IntVector2.GetDistance(Position.Pixel, EntityManager.PlayerEntity.Position.Pixel) < 64 + 32)
        {
            var angleToPlayer = Angle.GetDirection(this, EntityManager.PlayerEntity);
            MoveDirection.TurnTowardsAngle(angleToPlayer.Value);
            Speed.SetMoveSpeedToCurrentDirection();
        }
    }
}