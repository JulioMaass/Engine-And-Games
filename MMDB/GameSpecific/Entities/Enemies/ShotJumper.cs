using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using MMDB.GameSpecific.States.Enemy;
using Engine.Types;
using MMDB.GameSpecific.Entities.EnemyShots;

namespace MMDB.GameSpecific.Entities.Enemies;

public class ShotJumper : Entity
{
    public ShotJumper()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("ShotJumper", 32, 25);
        AddMmdbEnemyComponents(4, 4);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components
        SpawnManager.AddSpawnBehavior(new BehaviorSnapToFloor());
        AddMoveDirection();
        AddMoveSpeed(1f);
        AddJumpSpeed(5f);
        AddGravity();

        // Shooter Manager
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootAtPlayer());
        Shooter.RelativeSpawnPosition = IntVector2.New(0, -2);
        Shooter.ShotType = typeof(EnemyBullet);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 4f);

        // States
        AddStateManager();
        // Command States
        var stateJump = NewState(new StateEnemyJump());

        // Auto States
        var stateFall = NewState(new StateFall())
            .AddStateSettingBehavior(new BehaviorShoot())
            .AddToAutomaticStatesList();
        // Idle
        var stateIdle = NewState(new StateEnemyIdleGround())
            .AddToAutomaticStatesList();

        // Ai Control
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(new ConditionStateFrame(stateIdle, 30));
        AiControl.AddSingleStatePool(stateJump);
        AiControl.AddPermanentBehavior(new BehaviorFacePlayer());
    }
}