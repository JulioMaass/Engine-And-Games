using Candle.GameSpecific.Entities.EnemyShots;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Behaviors.State;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Candle.GameSpecific.Entities.Enemies;

public class RocketShooter : Entity
{
    public RocketShooter()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("RocketShooter");
        AddCenteredOutlinedCollisionBox();
        AddCandleEnemyComponents(2, 10);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        AddGravity();

        // Enemy specific components
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootStraight());
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        Shooter.ShotType = typeof(Rocket);
        Shooter.ShotModifiers.Add(e =>
        {
            e.Speed.MoveSpeed = 1f;
            e.Speed.Acceleration = 0.10f;
        });

        // States
        AddStateManager();
        // Auto States
        var idleState = NewState(new StateIdleAndFall())
            .AddBehavior(new BehaviorFacePlayer())
            .AddPostProcessingBehaviorWithConditions(GroupedBehaviors(new BehaviorShoot(), new BehaviorResetStateFrame()),
                new ConditionFrame(new RandomInt(105, 165), ComparisonType.Equal))
            .AddToAutomaticStatesList();
    }
}