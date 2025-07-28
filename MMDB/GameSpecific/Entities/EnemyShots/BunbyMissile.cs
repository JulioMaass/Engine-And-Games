using Engine.ECS.Entities.EntityCreation;
using MMDB.GameSpecific.States.Enemy;

namespace MMDB.GameSpecific.Entities.EnemyShots;

public class BunbyMissile : Entity
{
    public BunbyMissile()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("BunbyMissile", 25, 25);
        AddMmdbEnemyShotComponents(2);
        AddSolidBehavior();
        AddCollisionBox(11, 11, 5, 5);

        // TODO: Make it destructible
        // Enemy specific components
        AddMoveDirection();
        AddTurnSpeed(2); // TODO: 1.5f turn speed

        // States
        AddStateManager();
        // Auto States
        var state = NewStateWithTimedPattern(new StateEnemyTurnAngleAndMove(), (0, 4), (8, 4)) // TODO: Use DirectionFrames to draw it in different directions
            .AddToAutomaticStatesList();
    }
}