using Engine.ECS.Components.ControlHandling.Behaviors.CollisionBox;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.PositionHandling;
using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Enemies;

public class ShieldAttacker : Entity
{
    public ShieldAttacker()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("ShieldAttacker", 27, 32, 16, 16); // TODO: Check what happens when sprite/collision box are odd (flip behavior)
        AddMmdbEnemyComponents(4, 3);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);
        AddCenteredCollisionBox(16, 30);
        CollisionBox.BodyType = BodyType.FrontShield;

        // Enemy specific components
        AddMoveSpeed(2.0f);

        // States
        AddStateManager();
        // Auto States
        var stateMove = NewState(new StateDefault("Move"), 0, 4, 2)
            .AddBehavior(new BehaviorMoveToXFacing())
            .AddToAutomaticStatesList();
        // Command States
        var stateTurning = NewStateWithTimedPattern(new StateDefault("Turn"), (2, 4), (3, 4), (4, 4), (3, 4), (2, 4))
            .AddBehaviorWithConditions(new BehaviorMirrorXFacing(), new ConditionFrameEqual(12))
            .AddBehaviorWithConditions(new BehaviorChangeBodyType(BodyType.Vulnerable), new ConditionFrameEqual(4))
            .AddBehaviorWithConditions(new BehaviorChangeBodyType(BodyType.FrontShield), new ConditionFrameEqual(16))
            .AddKeepCondition(new ConditionFrameSmaller(20));

        //Ai Control
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(new ConditionState(stateMove), new ConditionFacingCollides());
        AiControl.AddSingleStatePool(stateTurning);
        var speedUpIfDamaged = NewBehavior(new BehaviorChangeMoveSpeed(3f)).AddCondition(new ConditionIsDamaged());
        AiControl.AddPermanentBehavior(speedUpIfDamaged);
    }
}