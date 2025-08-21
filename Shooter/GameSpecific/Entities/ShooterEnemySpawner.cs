using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace ShooterGame.GameSpecific.Entities;

public class ShooterEnemySpawner : Entity
{
    public ShooterEnemySpawner()
    {
        EntityKind = EntityKind.Gimmick;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("ShooterEnemy");
        Sprite.IsVisible = false;
        SpawnManager.DespawnOnScreenExit = false;

        // Shooter Manager
        Shooter = new Shooter(this);
        Shooter.AddShootAction(Shooter.ShootStill);
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        Shooter.ShotType = typeof(ShooterEnemy);

        // States
        AddStateManager();
        // Auto States
        var state = NewState()
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameLoop(200))
            .AddToAutomaticStatesList();
    }
}