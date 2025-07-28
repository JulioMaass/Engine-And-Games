using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.EnemyShots;

public class EnemyBullet : Entity
{
    public EnemyBullet()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("EnemyBullet", 8, 8);
        AddMmdbEnemyShotComponents(2);
        AddSolidBehavior();
        AddMoveDirection();

        // States
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}