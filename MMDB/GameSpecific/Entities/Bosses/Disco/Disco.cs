using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Bosses.Disco;

public class Disco : Entity
{
    public Disco()
    {
        EntityKind = EntityKind.Boss;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("Disco", 32, 32);
        AddMmdbEnemyComponents(28, 4);
        AddCollisionBox(16, 17, 8, 8);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        AddGravity();

        // States
        AddStateManager();
    }
}