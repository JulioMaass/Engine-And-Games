using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using SpaceMiner.GameSpecific.Entities.Ores;
using System;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidGreenWave : Entity
{
    private int FrameRandomizer;

    public AsteroidGreenWave()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("AsteroidGreen");
        AddCenteredOutlinedCollisionBox();
        AddSpaceMinerEnemyComponents(10, 1);
        AddSolidBehavior();
        //SpawnManager.DespawnOnScreenExit = false;
        AddItemDropper(
            (typeof(OreGreen), 1)
        );

        AddMoveSpeed(1f);
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

        FrameRandomizer = GetRandom.UnseededInt(360000);
    }

    protected override void CustomUpdate()
    {
        Speed.SetMoveSpeedToCurrentDirection();
        var cos = Math.Cos((FrameHandler.CurrentFrame + FrameRandomizer) / 12f) * 1.5f;
        Speed.AddXSpeed((float)cos);
    }
}