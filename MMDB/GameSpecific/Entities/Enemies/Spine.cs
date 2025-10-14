using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Snap;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.PositionHandling;
using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Enemies;

public class Spine : Entity
{
    public Spine()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("Spine", 22, 15, 11, 9);
        AddMmdbEnemyComponents(1, 2);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);
        AddCollisionBox(16, 11, 8, 6);
        CollisionBox.BodyType = BodyType.Shield;

        SpawnManager.AddSpawnBehavior(new BehaviorSnapToFloor());
        AddGravity();

        // TODO: Change direction, but don't change facing
        // TODO: Stop when getting shot

        // Enemy specific components
        AddMoveSpeed(1.0f);
        AddDashSpeed(2.0f);
        AddMoveDirection();

        // States
        AddStateManager();
        // Auto States
        var stateMove = NewState(new StateDefault("Move"), 0, 4, 3)
            .AddBehavior(new BehaviorMoveToXFacing())
            .AddBehaviorWithConditions(new BehaviorDashToXFacing(), new ConditionPlayerYDistanceRange(0, 16))
            .AddBehaviorWithConditions(new BehaviorMirrorXFacing(), new ConditionFacingCollides())
            .AddBehaviorWithConditions(new BehaviorMirrorXFacing(), new ConditionFacingLedge())
            .AddBehaviorWithConditions(new BehaviorMirrorXFacing(), new ConditionFacingOutOfRoom())
            .AddToAutomaticStatesList();
    }
}