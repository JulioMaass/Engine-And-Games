using Engine.ECS.Components.ControlHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Direction;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Snap;
using Engine.ECS.Components.ControlHandling.Behaviors.Directions;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;

namespace MMDB.GameSpecific.Entities.Enemies;

public class SuzyY : Entity
{
    public SuzyY()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("SuzyY", 16);
        AddMmdbEnemyComponents(4, 3);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components
        SpawnManager.AddSpawnBehavior(new BehaviorSnapOnAxis(Axis.Y));
        AddMoveDirection(90000);
        AddMoveSpeed(2.0f);

        // States
        AddStateManager();
        // Auto States
        var stateIdle = NewStateWithTimedPattern(default, (0, 5), (1, 5), (2, 60), (1, 5), (0, 5))
            .AddBehavior(new BehaviorStop())
            .AddToAutomaticStatesList();
        // Command States
        var stateMove = NewState()
            .AddStateSettingBehaviorWithConditions(new BehaviorReverseAngleDirection(MoveDirection), new ConditionCollidesWithSolidAtDirection())
            .AddKeepCondition(new ConditionCollidesWithSolidAtDirection().Reversed())
            .AddBehavior(new BehaviorMoveToCurrentDirection());
        StateManager.CommandState(stateIdle, 40); // To spawn with closed eye

        // Ai Control
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(new ConditionStateFrame(stateIdle, 80));
        AiControl.AddSingleStatePool(stateMove);
    }
}