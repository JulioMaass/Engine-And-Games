using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Bosses.Disco;

public class DiscoBigShot : Entity
{
    public DiscoBigShot()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("DiscoBigShot", 34, 34);
        AddMmdbEnemyShotComponents(4);
        AddCenteredCollisionBox(26);
        AddSolidBehavior();

        // States
        AddStateManager();
        NewState();
    }
}