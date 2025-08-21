using Engine.ECS.Components.ControlHandling.Behaviors.DeathAndDestroy;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.EnemyShots;

public class CannonBall : Entity
{
    public CannonBall()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("CannonBall", 10, 10);
        AddCandleEnemyShotComponents(10);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);
        AddMoveDirection();
        AddGravity();

        // States
        AddStateManager();
        var state = NewState()
        .AddBehaviorWithConditions(new BehaviorDestroy(), new ConditionFacingCollides());
        StateManager.AutomaticStatesList.Add(state);
    }
}