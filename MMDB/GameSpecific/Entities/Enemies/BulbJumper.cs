using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using MMDB.GameSpecific.Entities.EnemyShots;
using MMDB.GameSpecific.States.Enemy;

namespace MMDB.GameSpecific.Entities.Enemies;

public class BulbJumper : Entity
{
    public BulbJumper()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("BulbJumper", 20, 31);
        AddMmdbEnemyComponents(4, 4);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components
        SpawnManager.AddSpawnBehavior(new BehaviorSnapToFloor());
        AddMoveDirection();
        AddMoveSpeed(1.5f);
        AddJumpSpeed(5f);
        AddGravity();

        // Shooter Manager
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootLeftAndRight());
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 10);
        Shooter.ShotType = typeof(BulbSpark);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 3f);

        // States
        AddStateManager();
        // Auto States
        var stateFall = NewState(new StateFall())
            .AddToAutomaticStatesList();
        var stateIdle = NewState(new StateEnemyIdleGround())
            .AddStateSettingBehavior(new BehaviorShoot())
            .AddToAutomaticStatesList();
        // Command States
        var stateJump = NewState(new StateEnemyJump());

        // Ai Control
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(new ConditionStateFrame(stateIdle, 60));
        AiControl.AddSingleStatePool(stateJump);
        AiControl.AddPermanentBehavior(new BehaviorFacePlayer());
    }
}