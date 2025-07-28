using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.EnemyShots;

public class SpitFire : Entity
{
    public SpitFire()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("SpitFire", 16, 16);
        AddMmdbEnemyShotComponents(2);
        AddSolidBehavior();
        AddMoveDirection();

        // States
        AddStateManager();
        var state = NewState(default, 0, 8, 2);
        StateManager.AutomaticStatesList.Add(state);
    }
}