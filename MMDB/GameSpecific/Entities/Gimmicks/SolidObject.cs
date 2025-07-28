using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Gimmicks;

public class SolidObject : Entity
{
    public SolidObject()
    {
        EntityKind = EntityKind.Gimmick;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteTopLeftOrigin("DebugTiles", 64, 16);
        AddGimmickComponents(0, SolidType.Solid);
    }
}