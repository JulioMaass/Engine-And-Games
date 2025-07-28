using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Components.ShootingHandling.ShootActions;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using MMDB.GameSpecific.States.Enemy;

namespace MMDB.GameSpecific.Entities.Enemies.GasEnemies;

public class Lamp : Entity
{
    public Lamp()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("Lamp", 16, 28);
        AddMmdbEnemyComponents(4, 4);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components
        AddMoveDirection();
        AddMoveSpeed(1f);
        AddJumpSpeed(4f);
        AddGravity();
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootInAllDirections(8));
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        Shooter.ShotType = typeof(LampShot);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 3f);

        // Death
        DeathHandler.AddBehavior(new ShootInEightDirections());

        // TODO: Snap to floor on spawn

        // States
        AddStateManager();
        // Auto States
        var stateFall = NewState(new StateFall())
            .AddToAutomaticStatesList();
        var stateIdle = NewState(new StateEnemyIdleGround())
            .AddBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameEqual(30))
            .AddToAutomaticStatesList();
        // Command States
        var stateJump = NewState(new StateEnemyJump());
        var stateHover = NewState()
            .AddBehavior(new BehaviorStop())
            .AddBehaviorWithConditions(new BehaviorCancelCommandedState(), new ConditionPlayerXDistanceRange(0, 64));
        StateManager.CommandState(stateHover);

        // Ai Control
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(new ConditionStateFrame(stateIdle, 60));
        AiControl.AddSingleStatePool(stateJump);
        AiControl.AddPermanentBehavior(new BehaviorFacePlayer());
    }
}