using Candle.GameSpecific.Entities.EnemyShots;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Behaviors.State;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Types;

namespace Candle.GameSpecific.Entities.Enemies;

public class CannonTest : Entity
{
    public CannonTest() // Used for testing the Shooter component and parabolic movement shots
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
        Shooter.AddShootAction(() => Shooter.ShootParabolic(e => e.Physics.ParabolicMovement.LaunchAtEntityWithAngleSpeed(EntityManager.PlayerEntity, 6)));
        Shooter.RelativeSpawnPosition = IntVector2.New(8, 0);
        Shooter.ShotType = typeof(EyeShot);
        Shooter.ShotModifiers.Add(e => e.Speed.MoveSpeed = 5f);
        Shooter.ShotModifiers.Add(e => e.AddGravity());
        Shooter.ShotModifiers.Add(e => e.Gravity.Force = 0.25f);
        Shooter.ShotModifiers.Add(e => e.Gravity.GravityDir = Dir.Up);

        // States
        AddStateManager();
        // Auto States
        var idleState = NewState(new StateIdleAndFall())
            .AddBehavior(new BehaviorFacePlayer())
            .AddPostProcessingBehaviorWithConditions(GroupedBehaviors(new BehaviorShoot(), new BehaviorResetStateFrame()),
            new ConditionFrame(new RandomInt(90, 120), ComparisonType.Equal))
            .AddToAutomaticStatesList();
    }
}