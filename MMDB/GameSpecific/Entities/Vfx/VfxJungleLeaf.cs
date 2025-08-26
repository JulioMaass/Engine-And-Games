using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Vfx;

public class VfxJungleLeaf : Entity
{
    public VfxJungleLeaf()
    {
        EntityKind = EntityKind.DecorationVfx;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("LeafVfx", 8, 8, 4, 4);

        AddSpeed(0f, 0.25f);

        // States
        AddStateManager();
        var state = NewStateWithTimedPattern(default, (0, 8), (2, 8), (4, 8), (6, 8))
            .AddToAutomaticStatesList();

        AddVfxComponents(state.SpritePattern.Count);
    }
}