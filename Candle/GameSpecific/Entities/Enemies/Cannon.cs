using Candle.GameSpecific.Entities.EnemyShots;
using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Components.ControlHandling.States;
using Engine.Types;
using Microsoft.Xna.Framework;

namespace Candle.GameSpecific.Entities.Enemies;

public class Cannon : Entity
{
    public Cannon()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("Cannon", 25, 17);
        AddCenteredOutlinedCollisionBox();
        AddCandleEnemyComponents(2, 10);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootAtSpeed(new Vector2(3f, -1.5f)));
        Shooter.RelativeSpawnPosition = IntVector2.New(8, 0);
        Shooter.ShotType = typeof(CannonBall);
        Shooter.ShotModifiers.Add(e =>
        {
            e.Speed.MoveSpeed = 5f;
            e.Gravity.Force = 0.1f;
        });

        // States
        AddStateManager();
        // Auto States
        var idleState = NewState(new StateIdleAndFall())
            .AddBehavior(new BehaviorFacePlayer())
            .AddBehaviorWithConditions(GroupedBehaviors(new BehaviorShoot(), new BehaviorResetFrame()),
            new ConditionFrame(new RandomInt(90, 120), ComparisonType.Equal))
            .AddToAutomaticStatesList();
    }
}