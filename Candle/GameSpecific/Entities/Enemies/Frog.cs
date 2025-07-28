using Candle.GameSpecific.States.Enemy;
using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Components.ControlHandling.States;
using Engine.Types;

namespace Candle.GameSpecific.Entities.Enemies;

public class Frog : Entity
{
    public Frog()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("Frog", 64, 64);
        AddCenteredCollisionBox(16);
        AddCandleEnemyComponents(3, 10);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components
        SpawnManager.AddSpawnBehavior(new BehaviorSnapToFloor());
        AddMoveSpeed(1.25f);
        AddGravity();

        // States
        AddStateManager();
        // Override States
        var stateHurt = NewState(default, 1)
            .AddStartCondition(new ConditionKnockedBack())
            .AddKeepCondition(new ConditionFrameSmaller(15))
            .AddStateSettingBehavior(new BehaviorStop())
            .AddStateSettingBehavior(new BehaviorFacePlayer())
            .AddBehavior(new BehaviorKnockbackMovement())
            .AddToOverrideStatesList();

        // Command States
        var stateJumpLow = NewState(new StateEnemyJump(4f), 1);
        var stateJumpHigh = NewState(new StateEnemyJump(5.5f), 1);

        // Auto States
        var stateFall = NewState(new StateFall(), 1)
            .AddBehavior(new BehaviorFacePlayer())
            .AddToAutomaticStatesList();
        // Idle
        var stateIdle = NewState(new StateEnemyIdleGround())
            .AddBehavior(new BehaviorFacePlayer())
            .AddToAutomaticStatesList();

        // Ai Control
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(
            new ConditionState(stateIdle),
            new ConditionFrame(new RandomInt(30, 45), ComparisonType.Equal));
        AiControl.AddSingleStatePool(stateJumpLow);
        AiControl.AddSingleStatePool(stateJumpHigh);
    }
}