using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.StageHandling;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidBlueTeleport : Entity
{
    public AsteroidBlueTeleport()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("AsteroidBlue", 32);
        AddSpriteVariation(4, 1);
        AddCenteredCollisionBox(16);
        AddSpaceMinerEnemyComponents(10, 1);
        AddSolidBehavior();
        //SpawnManager.DespawnOnScreenExit = false;
        AddItemDropper(
            (typeof(OreBlue), 1)
        );

        AddMoveSpeed(2f);
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
    }

    protected override void CustomUpdate()
    {
        if (Position.Pixel.X < StageManager.CurrentRoom.PositionInPixels.X)
            Position.SetPixelX(StageManager.CurrentRoom.PositionInPixels.X + StageManager.CurrentRoom.SizeInPixels.X);
        if (Position.Pixel.X > StageManager.CurrentRoom.PositionInPixels.X + StageManager.CurrentRoom.SizeInPixels.X)
            Position.SetPixelX(StageManager.CurrentRoom.PositionInPixels.X);
        if (Position.Pixel.Y < StageManager.CurrentRoom.PositionInPixels.Y)
            Position.SetPixelY(StageManager.CurrentRoom.PositionInPixels.Y + StageManager.CurrentRoom.SizeInPixels.Y);
        if (Position.Pixel.Y > StageManager.CurrentRoom.PositionInPixels.Y + StageManager.CurrentRoom.SizeInPixels.Y)
            Position.SetPixelY(StageManager.CurrentRoom.PositionInPixels.Y);
    }
}