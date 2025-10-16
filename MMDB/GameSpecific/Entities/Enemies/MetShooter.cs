using Engine.ECS.Components.ControlHandling.Behaviors.CollisionBox;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Snap;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.PositionHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using MMDB.GameSpecific.Entities.EnemyShots;
using MMDB.GameSpecific.States.Enemy;

namespace MMDB.GameSpecific.Entities.Enemies;

public class MetShooter : Entity
{
    public MetShooter()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("MetShooter", 19, 14);
        AddMmdbEnemyComponents(1, 3);
        AddCollisionBox(16, 10, 8, 4);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components
        SpawnManager.AddSpawnBehavior(new BehaviorSnapToFloor());
        AddGravity();

        // Shooter
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootSpread(0, 45000));
        Shooter.AmountOfShots = 3;
        Shooter.RelativeSpawnPosition = IntVector2.New(4, 4);
        Shooter.ShotType = typeof(EnemyBullet);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 2.5f);

        // States
        AddStateManager();
        // Auto States
        var stateHidden = NewState()
            .AddStateSettingBehavior(new BehaviorChangeBodyType(BodyType.Shield)) // TODO: Change collision box size when he gets up/down
            .AddToAutomaticStatesList();
        // Command States
        var stateShoot = NewStateWithTimedPattern(new StateEnemyKeepForDuration(60), (1, 4), (2, 52), (1, 4))
            .AddStateSettingBehavior(new BehaviorChangeBodyType(BodyType.Vulnerable))
            .AddPostProcessingBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameEqual(30));

        // Ai Control
        // Get up
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(new ConditionStateFrame(stateHidden, 60),
            new ConditionPlayerXDistanceRange(0, 128));
        AiControl.AddSingleStatePool(stateShoot);
        AiControl.AddPermanentBehavior(new BehaviorFacePlayer());
    }
}