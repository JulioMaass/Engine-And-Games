using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidOrange : Asteroid
{
    public AsteroidOrange()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("AsteroidOrange", 32);
        AddSpriteVariation(4, 1);
        AddCenteredCollisionBox(16);
        AddSpaceMinerEnemyComponents(50, 50);
        AddSolidBehavior();
        //SpawnManager.DespawnOnScreenExit = false;
        AddItemDropper(8, (typeof(OreOrange), 1), (typeof(OreGray), 2));

        BloomSource = new BloomSource(this, 0.80f);

        AddMoveSpeed(1f);
        Speed.Acceleration = 0.08f;
        Speed.MaxSpeed = 2f;
        AddMoveDirection();
        //AddDeathHandler(new BehaviorAddScore(1));

        SpriteVfxs = new(this);
        SpriteVfxs.Add("MegaCircle", 128, CustomColor.PicoOrange, 0.0125f);
        SpriteVfxs.Add("MegaCircle", 126, CustomColor.PicoOrange, 0.0125f);
        SpriteVfxs.Add("MegaCircle", 96, CustomColor.PicoOrange, 0.0125f);
        SpriteVfxs.Add("MegaCircle", 94, CustomColor.PicoOrange, 0.0125f);
        SpriteVfxs.Add("MegaCircle", 64, CustomColor.PicoOrange, 0.0125f);
        SpriteVfxs.Add("MegaCircle", 62, CustomColor.PicoOrange, 0.0125f);

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
        Speed.TurnSpeed = 100;
        if (IntVector2.GetDistance(Position.Pixel, EntityManager.PlayerEntity.Position.Pixel) < 128)
            Speed.TurnSpeed = 500;
        if (IntVector2.GetDistance(Position.Pixel, EntityManager.PlayerEntity.Position.Pixel) < 64)
            Speed.TurnSpeed = 1000;
        if (IntVector2.GetDistance(Position.Pixel, EntityManager.PlayerEntity.Position.Pixel) < 32 + 16)
            Speed.TurnSpeed = 1500;
        if (IntVector2.GetDistance(Position.Pixel, EntityManager.PlayerEntity.Position.Pixel) < 32)
            Speed.TurnSpeed = 2000;
        // Turn towards the player
        //if (IntVector2.GetDistance(Position.Pixel, EntityManager.PlayerEntity.Position.Pixel) < 64 + 32)
        //{
        var angleToPlayer = Angle.GetDirection(this, EntityManager.PlayerEntity);
        MoveDirection.TurnTowardsAngle(angleToPlayer.Value);
        Speed.SetMoveSpeedToCurrentDirection();
        //}
    }
}