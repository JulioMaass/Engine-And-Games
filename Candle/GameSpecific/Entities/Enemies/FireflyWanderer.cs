using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Candle.GameSpecific.Entities.Enemies;

public class FireflyWanderer : Entity
{
    public FireflyWanderer()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("Firefly");
        AddCenteredOutlinedCollisionBox();
        AddCandleEnemyComponents(2, 10);
        AddSolidBehavior();

        // Enemy specific
        AddMoveDirection();
        AddSpeed();
        Speed.Acceleration = 0.05f;
        Speed.MaxSpeed = 1.5f;

        // States
        AddStateManager();

        // Auto States
        var state = NewState()
            .AddBehaviorWithConditions(
                new BehaviorMirrorXFacing(),
                new ConditionXDistanceFromSpawnBiggerThan(48), // TODO: Change reference point (starting position) if enemy is out of reach from this distance (knocked from a platform, etc.)
                new ConditionXFacingAwayFromSpawn())
            .AddBehavior(new BehaviorSetDirectionToXFacing())
            .AddBehavior(new BehaviorAccelerateToDirection())
            .AddToAutomaticStatesList();
    }
}