using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Snap;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Candle.GameSpecific.Entities.Enemies;

public class Rat : Entity
{
    public Rat()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("Rat", 22, 15, 13, 9);
        AddCenteredCollisionBox(10, 10);
        AddCandleEnemyComponents(2, 10);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components
        SpawnManager.AddSpawnBehavior(new BehaviorFacePlayer());
        SpawnManager.AddSpawnBehavior(new BehaviorSnapToFloor());
        SpawnManager.DespawnDelay = 300;
        AddMoveSpeed(1.1f);
        AddGravity();

        // States
        AddStateManager();

        // Override States
        var stateHurt = NewState(default, 0)
            .AddStartCondition(new ConditionKnockedBack())
            .AddKeepCondition(new ConditionFrameSmaller(15))
            .AddStateSettingBehavior(new BehaviorStop())
            .AddStateSettingBehavior(new BehaviorFacePlayer())
            .AddBehavior(new BehaviorKnockbackMovement())
            .AddToOverrideStatesList();

        // Auto States
        var stateIdle = NewState()
            .AddBehavior(new BehaviorStop(Engine.Helpers.Axes.X))
            .AddToAutomaticStatesList();

        // Command states
        var stateWalk = NewState(default, 1)
            // May face player when starts walking
            .AddStateSettingBehaviorWithConditions( // TODO: Require label when making a custom state/behavior for readability and debugging?
                new BehaviorMirrorXFacing(),
                new ConditionXFacingPlayer().Reversed(),
                new ConditionChance(66))
            // Invert direction if moving too far from spawn
            .AddBehaviorWithConditions(
                new BehaviorMirrorXFacing(),
                new ConditionXDistanceFromSpawnBiggerThan(new RandomInt(16, 64)), // TODO: Change reference point (starting position) if enemy is out of reach from this distance (knocked from a platform, etc.)
                new ConditionXFacingAwayFromSpawn())
            // Reverse direction if colliding with wall
            .AddBehaviorWithConditions(new BehaviorMirrorXFacing(), new ConditionFacingCollides())
            // Duration
            .AddKeepCondition(new ConditionFrame(new RandomInt(30, 120).SetRollsOnFalse(), ComparisonType.LessOrEqual))
            // Move
            .AddBehavior(new BehaviorMoveToXFacing());

        // Ai Control
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(
            new ConditionState(stateIdle),
            new ConditionFrame(new RandomInt(30, 60), ComparisonType.Equal));
        AiControl.AddSingleStatePool(stateWalk);
    }
}