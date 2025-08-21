using Engine.ECS.Components.ControlHandling.Behaviors.CollisionBox;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Snap;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.StateGrouping;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.PositionHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using MMDB.GameSpecific.Entities.EnemyShots;
using MMDB.GameSpecific.States.Enemy;

namespace MMDB.GameSpecific.Entities.Enemies;

public class MetJumper : Entity
{
    public MetJumper()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("MetJumper", 19, 22);
        AddMmdbEnemyComponents(1, 3);
        AddCollisionBox(16, 10, 8, 4);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components
        SpawnManager.AddSpawnBehavior(new BehaviorSnapToFloor());
        AddMoveDirection();
        AddMoveSpeed(1.0f);
        AddJumpSpeed(4.5f);
        AddGravity();

        // Shooter Manager
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootSpread(0, 30000));
        Shooter.AmountOfShots = 3;
        Shooter.RelativeSpawnPosition = IntVector2.New(4, 4);
        Shooter.ShotType = typeof(EnemyBullet);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 2f);

        // States
        AddStateManager();
        // Auto States
        var stateHidden = NewState(new StateDefault("Hidden"))
            .AddStateSettingBehavior(new BehaviorChangeBodyType(BodyType.Shield)) // TODO: Change collision box size when he gets up/down
            .AddToAutomaticStatesList();
        // Command States
        var stateGetUp = NewStateWithTimedPattern(new StateDefault("GetUp"), (1, 4), (2, 6))
            .AddStateSettingBehavior(new BehaviorChangeBodyType(BodyType.Vulnerable))
            .AddKeepCondition(new ConditionFrameSmaller(10));
        var stateJumpAndFall = NewState(new StateEnemyJumpAndFall(), 3);
        var stateShootAndGetDown = NewStateWithTimedPattern(new StateDefault("ShootAndGetDown"), (2, 56), (1, 4))
            .AddStateSettingBehavior(new BehaviorStop())
            .AddKeepCondition(new ConditionFrameSmaller(60))
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameEqual(30));

        // Ai Control
        // Get up
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(new ConditionStateFrame(stateHidden, 60),
            new ConditionPlayerXDistanceRange(0, 128));
        var pattern = new Pattern(stateGetUp, stateJumpAndFall, stateShootAndGetDown);
        var patternPool = new PatternPool(pattern);
        AiControl.PatternPools.Add(patternPool);
        AiControl.AddPermanentBehavior(new BehaviorFacePlayer());
    }
}