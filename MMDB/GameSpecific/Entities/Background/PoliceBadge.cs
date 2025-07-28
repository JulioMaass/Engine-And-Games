using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Background;

public class PoliceBadge : Entity
{
    public PoliceBadge()
    {
        EntityKind = EntityKind.Background;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("PoliceBadge");
        AddGimmickComponents(0, SolidType.Solid);
    }
}