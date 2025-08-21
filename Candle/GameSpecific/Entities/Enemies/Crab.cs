using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Enemies;

public class Crab : Entity
{
    public Crab()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("Crab", 26, 14);
        AddCenteredCollisionBox(12, 12);
        AddCandleEnemyComponents(2, 10);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components
        AddMoveSpeed(2f);

        // States
        AddStateManager();

        // TODO: Should have gravity and StateFall after knockback
        var stateHurt = NewState()
            .AddStartCondition(new ConditionKnockedBack())
            .AddKeepCondition(new ConditionFrameSmaller(15))
            .AddStateSettingBehavior(new BehaviorStop())
            .AddStateSettingBehavior(new BehaviorFacePlayer())
            .AddBehavior(new BehaviorKnockbackMovement())
            .AddToOverrideStatesList();

        // Auto States
        var crawlState = NewState(default, 0, 8, 2)
            .AddStateSettingBehavior(new SettingBehaviorGoDownAndCrawlTowardsPlayer())
            .AddToAutomaticStatesList();
        crawlState.MovementType = MovementType.Crawling; // TODO: Check and improve crawling code (should be inside state? crawling direction should be set automatically sometimes (for example, if the enemy is touching ground and wants to walk towards player)
    }
}