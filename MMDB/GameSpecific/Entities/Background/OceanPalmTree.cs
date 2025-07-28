using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Background;

public class OceanPalmTree : Entity
{
    public OceanPalmTree()
    {
        EntityKind = EntityKind.Background;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("OceanPalmTree");
    }
}