using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Entities.Shared;
using Microsoft.Xna.Framework;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidRedBlast : Entity
{
    public AsteroidRedBlast()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("AsteroidRed", 32);
        AddSpriteVariation(4, 1);
        AddCenteredCollisionBox(16);
        AddSpaceMinerEnemyComponents(50, 1);
        AddSolidBehavior();
        //SpawnManager.DespawnOnScreenExit = false;
        AddItemDropper(
            (typeof(OreRed), 1)
        );

        BloomSource = new BloomSource(this, 0.8f);

        AddRandomMoveSpeed(0.4f, 0.6f);
        Speed.Acceleration = 0.08f;
        Speed.MaxSpeed = 8f;
        AddMoveDirection();
        //AddDeathHandler(new BehaviorAddScore(1));+

        //// Shooter Manager
        //Shooter = new Shooter(this);
        //Shooter.AddShootAction(() => Shooter.ShootAtPlayer());
        //Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        //Shooter.ShotType = typeof(ShooterEnemyShot);
        //Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 2f);

        var duration = 30;
        var damage = 1;
        var size = 48;
        var color = new Color(255, 0, 0, 255);
        AddDeathHandler(new BehaviorCreateBlast(typeof(ResizableBlast), EntityKind.EnemyShot, AlignmentType.Hostile, duration, damage, size, color));

        // States
        AddStateManager();
        // Auto States
        var state = NewState()
            .AddToAutomaticStatesList();
    }

    protected override void CustomUpdate()
    {
    }
}