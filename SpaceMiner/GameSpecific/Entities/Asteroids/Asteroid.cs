using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Entities.EntityCreation;
using SpaceMiner.GameSpecific.Entities.Ores;
using SpaceMiner.GameSpecific.Entities.Vfx;

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
        AddSpaceMinerEnemyComponents(30, 1);
        AddSolidBehavior();
        //SpawnManager.DespawnOnScreenExit = false;
        AddItemDropper(
            (typeof(OreGray), 1)
        );

        AddRandomMoveSpeed(1.25f, 2.0f);
        Speed.Acceleration = 0.08f;
        Speed.MaxSpeed = 8f;
        AddMoveDirection();
        AddDeathHandler(new BehaviorCreateEntity(typeof(VfxDebris)));
        //AddDeathHandler(new BehaviorCreateEntities(typeof(VfxSmokeEmitter), typeof(VfxBlastEmitter)));

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