using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Enemies;

public class Bat : Entity
{
    public Bat()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("Bat", 44, 36, 22, 18);
        AddCollisionBox(30, 12, 15, 6);
        AddCandleEnemyComponents(2, 10);
        AddSolidBehavior();

        // Enemy specific components
        AddMoveDirection();
        AddMoveSpeed(1.0f);
        Speed.Acceleration = 0.04f;
        Speed.MaxSpeed = 1.25f;

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
        var state = NewStateWithTimedPattern(default, (0, 6), (1, 6), (2, 4), (3, 6))
            .AddBehavior(new BehaviorSetDirectionToPlayer())
            .AddBehavior(new BehaviorAccelerateToDirection())
            .AddToAutomaticStatesList();

        // Ai Control
        AiControl.AddPermanentBehavior(new BehaviorFacePlayer());
    }
}