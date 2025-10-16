using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Snap;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.StateGrouping;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using MMDB.GameSpecific.Entities.EnemyShots;
using MMDB.GameSpecific.States.Enemy;

namespace MMDB.GameSpecific.Entities.Enemies;

public class BulbJumperBossLike : Entity
{
    public BulbJumperBossLike()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("BulbJumper", 20, 31);
        AddMmdbEnemyComponents(4, 4);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components
        SpawnManager.AddSpawnBehavior(new BehaviorSnapToFloor());
        AddMoveDirection();
        AddMoveSpeed(1.5f);
        AddJumpSpeed(5f);
        AddGravity();

        // Shooter Manager
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootLeftAndRight());
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 10);
        Shooter.ShotType = typeof(BulbSpark);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 3f);

        // States
        AddStateManager();
        // Auto States
        var stateFall = NewState(new StateFall())
            .AddToAutomaticStatesList();
        var stateIdle = NewState(new StateEnemyIdleGround())
            .AddPostProcessingStateSettingBehavior(new BehaviorShoot())
            .AddToAutomaticStatesList();
        // Command States
        var stateJump = NewState(new StateEnemyJump());

        // Ai Control
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(new ConditionStateFrame(stateIdle, 60));
        var patternJumpOnce = new Pattern(stateJump);
        var patternJumpTwice = new Pattern(stateJump, stateJump);
        var patternPoolNear = new PatternPool(patternJumpOnce, patternJumpOnce, patternJumpOnce, patternJumpOnce, patternJumpTwice);
        patternPoolNear.Owner = this;
        var distanceFilter = 192;
        patternPoolNear.AddCondition(new ConditionPlayerXDistanceRange(0, distanceFilter));
        var patternPoolFar = new PatternPool(patternJumpOnce, patternJumpTwice, patternJumpTwice, patternJumpTwice, patternJumpTwice);
        patternPoolFar.Owner = this;
        patternPoolFar.AddCondition(new ConditionPlayerXDistanceRange(distanceFilter, int.MaxValue));
        AiControl.PatternPools.Add(patternPoolNear);
        AiControl.PatternPools.Add(patternPoolFar);
        AiControl.AddPermanentBehavior(new BehaviorFacePlayer());
    }
}