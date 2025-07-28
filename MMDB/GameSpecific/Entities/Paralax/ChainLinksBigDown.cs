using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.StageHandling;

namespace MMDB.GameSpecific.Entities.Paralax;

public class ChainLinksBigDown : Entity
{
    public ChainLinksBigDown()
    {
        EntityKind = EntityKind.Paralax;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("ChainLinks");
        AddParalax(LayerId.ParalaxTiles, (0.5f, 0.0f), (-0.0f, 0.4f));
        Paralax.Repeat = ParalaxRepeat.Both;
        Paralax.RepeatDistance = (64, 0);
    }
}