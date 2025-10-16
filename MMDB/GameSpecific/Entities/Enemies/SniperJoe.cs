using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Snap;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using MMDB.GameSpecific.Entities.EnemyShots;

namespace MMDB.GameSpecific.Entities.Enemies;

public class SniperJoe : Entity
{
    public SniperJoe()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("SniperJoe", 30, 25);
        AddMmdbEnemyComponents(6, 4);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);
        AddCollisionBox(12, 21, 6, 9); // TODO: Add shield collision box

        // Enemy specific components
        SpawnManager.AddSpawnBehavior(new BehaviorSnapToFloor());
        AddGravity();

        // Shooter Manager
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootAtPlayer());
        Shooter.RelativeSpawnPosition = IntVector2.New(13, 3);
        Shooter.ShotType = typeof(EnemyBullet);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 3f);

        // States
        AddStateManager();
        // Auto States
        var stateIdle = NewStateWithTimedPattern(new StateDefault("Idle"), (0, 28), (1, 4), (2, 16), (3, 4), (4, 28), (5, 22), (6, 6), (7, 12))
            .AddToAutomaticStatesList();
        // Command States
        var stateShoot = NewStateWithTimedPattern(new StateDefault("Shoot"), (8, 8), (9, 104), (8, 8))
            .AddKeepCondition(new ConditionFrameSmaller(120))
            .AddPostProcessingBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameEqual(20))
            .AddPostProcessingBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameEqual(60))
            .AddPostProcessingBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameEqual(100));

        //Ai Control
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(new ConditionStateFrame(stateIdle, 120));
        AiControl.AddSingleStatePool(stateShoot);
        AiControl.AddPermanentBehavior(new BehaviorFacePlayer());
    }
}