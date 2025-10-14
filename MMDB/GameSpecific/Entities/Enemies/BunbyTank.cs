using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Snap;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using MMDB.GameSpecific.Entities.EnemyShots;

namespace MMDB.GameSpecific.Entities.Enemies;

public class BunbyTank : Entity
{
    public BunbyTank()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("BunbyTank", 46, 32, 19, 17);
        AddMmdbEnemyComponents(10, 4);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);
        AddCollisionBox(22, 28, 11, 14);

        // Enemy specific components
        SpawnManager.AddSpawnBehavior(new BehaviorSnapToFloor());
        AddGravity();

        // Enemy specific components
        AddMoveSpeed(1.0f);
        AddMoveDirection();

        // Shooter Manager // TODO: Mirror shot's direction
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootStraight());
        Shooter.RelativeSpawnPosition = IntVector2.New(28, -6); // TODO: Add secondary spawn point
        Shooter.ShotType = typeof(BunbyMissile);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 1.8f);

        // States
        AddStateManager();
        // Auto States
        var stateMove = NewState(new StateDefault("Move"), 0, 4, 3)
            .AddBehavior(new BehaviorMoveToXFacing())
            .AddBehaviorWithConditions(new BehaviorMirrorXFacing(), new ConditionFacingCollides())
            .AddBehaviorWithConditions(new BehaviorMirrorXFacing(), new ConditionFacingLedge())
            .AddBehaviorWithConditions(new BehaviorMirrorXFacing(), new ConditionFacingOutOfRoom())
            .AddToAutomaticStatesList();

        // Command States
        var stateShoot = NewStateWithTimedPattern(new StateDefault("Shoot"), (3, 10), (4, 5), (5, 10), (4, 5), (3, 10), (6, 5), (7, 10), (6, 5), (3, 10))
            .AddKeepCondition(new ConditionFrameSmaller(70))
            .AddBehavior(new BehaviorStop())
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameEqual(20))
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameEqual(50));

        //Ai Control
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(new ConditionStateFrame(stateMove, 120));
        AiControl.AddSingleStatePool(stateShoot);
    }
}