using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Candle.GameSpecific.Entities.Hazards;

public class TriggerArrowShooter : Entity
{
    public TriggerArrowShooter()
    {
        EntityKind = EntityKind.Gimmick;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("ArrowShooter", 16);
        AddCenteredCollisionBox(14);
        AddCandleGimmickComponents();

        // Shooter
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootStraight());
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        Shooter.ShotType = typeof(Arrow);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 4f);

        // States
        AddStateManager();
        var stateTriggered = NewState()
            .AddKeepCondition(new ConditionFrameSmaller(60))
            .AddPostProcessingBehaviorWithConditions(new BehaviorShoot(), new ConditionFrameEqual(30))
            .AddToAutomaticStatesList();
        var stateIdle = NewState()
            .AddToAutomaticStatesList();
        stateTriggered
            .AddStartCondition(new ConditionState(stateIdle))
            .AddStartCondition(new ConditionXFacingPlayer())
            .AddStartCondition(new ConditionPlayerYDistanceRange(0, 8));

        // Invert if wall is on the right
        SpawnManager.AddSpawnBehavior(new BehaviorFaceAwayFromWall());

    }
}