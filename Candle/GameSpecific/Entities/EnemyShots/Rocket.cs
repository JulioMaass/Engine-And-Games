using Engine.ECS.Components.ControlHandling.Behaviors.DeathAndDestroy;
using Engine.ECS.Components.ControlHandling.Behaviors.Shoot;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Candle.GameSpecific.Entities.EnemyShots;

public class Rocket : Entity
{
    public Rocket()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("Rocket");
        AddCandleEnemyShotComponents(10);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);
        AddMoveDirection();

        // Shooter
        Shooter = new Shooter(this);
        Shooter.AddShootAction(() => Shooter.ShootStill());
        Shooter.RelativeSpawnPosition = IntVector2.New(0, 0);
        Shooter.ShotType = typeof(Explosion48);
        Shooter.ShotModifiers.Add(e =>
        {
        });

        // States
        AddStateManager();
        var state = NewState()
            .AddBehavior(new BehaviorAccelerateToDirection())
            .AddPostProcessingBehaviorWithConditions(GroupedBehaviors(new BehaviorShoot(), new BehaviorDestroy()), new ConditionFacingCollides());
        StateManager.AutomaticStatesList.Add(state);
    }
}