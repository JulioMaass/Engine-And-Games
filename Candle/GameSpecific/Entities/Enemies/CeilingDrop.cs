using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Enemies;

public class CeilingDrop : Entity
{
    public CeilingDrop()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("CeilingDrop");
        AddCenteredOutlinedCollisionBox();
        AddCandleEnemyComponents(2, 10);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components
        SpawnManager.AddSpawnBehavior(new BehaviorFacePlayer());
        AddMoveSpeed(1.25f);
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
        var stateIdle = NewState(new StateFall())
            .AddBehavior(new BehaviorStop(Engine.Helpers.Axes.X))
            .AddToAutomaticStatesList();
        var stateWalk = NewState()
            .AddBehaviorWithConditions(new BehaviorMirrorXFacing(), new ConditionFacingCollides())
            .AddBehavior(new BehaviorMoveToXFacing())
            .AddToAutomaticStatesList();
    }
}