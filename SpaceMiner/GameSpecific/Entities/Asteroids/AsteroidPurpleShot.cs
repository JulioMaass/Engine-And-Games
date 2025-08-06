using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities.EntityCreation;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidPurpleShot : Entity
{
    public AsteroidPurpleShot()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("PurpleShot");
        AddCenteredOutlinedCollisionBox();
        AddAlignment(AlignmentType.Hostile);
        AddDamageDealer(50);
        AddSolidBehavior();
        //SpawnManager.DespawnOnScreenExit = false;
        //AddItemDropper(
        //    (typeof(ShooterMachineGunItem), 9)
        //);

        AddRandomMoveSpeed(2.0f, 2.5f);
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