using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
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
        AddDamageTaker(1);
        AddSolidBehavior();
        AddCollisionBox(11, 11, 5, 5);

        // Sprite rotation
        Sprite.DirectionOffset = (8, 2);

        // Enemy specific components
        AddMoveDirection();
        AddTurnSpeed(2000);
        AddFrameCounter(300, true);
        AddDeathHandler(new BehaviorCreateEntity(typeof(ExplosionSmall)));

        // States
        AddStateManager();
        // Auto States
        var state = NewStateWithTimedPattern(new StateEnemyTurnAngleAndMove(), (0, 4), (1, 4))
            .AddToAutomaticStatesList();
    }
}