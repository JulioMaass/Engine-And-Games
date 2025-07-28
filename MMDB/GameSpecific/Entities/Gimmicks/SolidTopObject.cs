using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Gimmicks;

public class SolidTopObject : Entity
{
    public SolidTopObject()
    {
        EntityKind = EntityKind.Gimmick;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteTopLeftOrigin("DebugTiles", 64, 16, 0, 48);
        AddGimmickComponents(0, SolidType.SolidTop);
    }
}