using Engine.ECS.Components.ControlHandling.Behaviors.Direction;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Enemies;

public class FireflyChaser : Entity
{
    public FireflyChaser()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("FireflyChaser");
        AddCenteredOutlinedCollisionBox();
        AddCandleEnemyComponents(2, 10);
        AddSolidBehavior();

        // Enemy specific
        AddMoveDirection();
        AddSpeed();
        Speed.Acceleration = 0.05f;
        Speed.MaxSpeed = 2.0f;

        // States
        AddStateManager();

        // Auto States
        var state = NewState()
            .AddBehavior(new BehaviorFacePlayer())
            .AddBehavior(new BehaviorSetDirectionToXFacing())
            .AddBehavior(new BehaviorAccelerateToDirection())
            .AddToAutomaticStatesList();
    }
}