using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Candle.GameSpecific.Entities.Hazards;

public class TimedArrowShooter : Entity
{
    public TimedArrowShooter()
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
        var idleState = NewState()
            .AddBehaviorWithConditions(GroupedBehaviors(new BehaviorShoot(), new BehaviorResetFrame()),
                new ConditionFrame(90, ComparisonType.Equal))
            .AddToAutomaticStatesList();

        // Invert if wall is on the right
        SpawnManager.AddSpawnBehavior(new BehaviorFaceAwayFromWall());
    }
}