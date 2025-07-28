using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Bosses.Disco;

public class DiscoSmallShot : Entity
{
    public DiscoSmallShot()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("DiscoSmallShot", 26, 26);
        AddMmdbEnemyShotComponents(3);
        AddCenteredCollisionBox(20);
        AddSolidBehavior();

        // States
        AddStateManager();
        NewState();
    }
}