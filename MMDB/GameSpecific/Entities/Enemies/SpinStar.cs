using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Direction;
using Engine.ECS.Components.ControlHandling.Behaviors.Direction;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Behaviors.Targeting;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using MMDB.GameSpecific.Entities.EnemyShots;

namespace MMDB.GameSpecific.Entities.Enemies;

public class SpinStar : Entity
{
    public SpinStar()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("SpinStar", 24);
        AddMmdbEnemyComponents(1, 2);
        AddCenteredCollisionBox(18);
        AddSolidBehavior();

        // Enemy specific components
        AddMoveDirection();
        AddMoveSpeed(1.5f);
        AddTurnSpeed(2000);
        AddMoveDirection();
        TargetPool = new TargetPool(this);

        // Shooter
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootAimedSpread(72000));
        Shooter.AmountOfShots = 5;
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        Shooter.ShotType = typeof(EnemyBullet);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 3f);
        AddShootDirection();

        // States
        AddStateManager();
        // Auto States
        var stateDash = NewState(default, 4)
            .AddStateSettingBehavior(new BehaviorTargetNearestEntity(AlignmentType.Friendly, EntityKind.Player))
            .AddStateSettingBehavior(new BehaviorSetDirectionToTarget(8))
            .AddStateSettingBehavior(new BehaviorMoveToCurrentDirection())
            .AddBehaviorWithConditions(new BehaviorDecelerateMomentum(40), new ConditionFrame(2, ComparisonType.Greater))
            .AddToAutomaticStatesList();
        // Command States
        var shootFrame = 50;
        var shootStateDuration = 80;
        var stateAimUp = NewState(default, 0)
            .AddStateSettingBehavior(new BehaviorFacePlayer())
            .AddStateSettingBehavior(new BehaviorSetShootDirection(270000))
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameEqual(shootFrame))
            .AddKeepCondition(new ConditionFrameSmaller(shootStateDuration));
        var stateAimForward = NewState(default, 1)
            .AddStateSettingBehavior(new BehaviorFacePlayer())
            .AddStateSettingBehavior(new BehaviorSetShootDirection(0))
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameEqual(shootFrame))
            .AddKeepCondition(new ConditionFrameSmaller(shootStateDuration));
        var stateAimDown = NewState(default, 2)
            .AddStateSettingBehavior(new BehaviorFacePlayer())
            .AddStateSettingBehavior(new BehaviorSetShootDirection(90000))
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameEqual(shootFrame))
            .AddKeepCondition(new ConditionFrameSmaller(shootStateDuration));
        var stateAimBack = NewState(default, 3)
            .AddStateSettingBehavior(new BehaviorFacePlayer())
            .AddStateSettingBehavior(new BehaviorSetShootDirection(180000))
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameEqual(shootFrame))
            .AddKeepCondition(new ConditionFrameSmaller(shootStateDuration));

        // Ai Control
        // Get up
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(new ConditionStateFrame(stateDash, 45));
        AiControl.AddSingleStatePool(stateAimUp);
        AiControl.AddSingleStatePool(stateAimForward);
        AiControl.AddSingleStatePool(stateAimDown);
        AiControl.AddSingleStatePool(stateAimBack);
    }
}