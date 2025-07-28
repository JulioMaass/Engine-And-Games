using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.StageHandling;

namespace MMDB.GameSpecific.Entities.Paralax;

public class ChainLinksSmallUp : Entity
{
    public ChainLinksSmallUp()
    {
        EntityKind = EntityKind.Paralax;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("ChainLinksSmall");
        AddParalax(LayerId.ParalaxTiles, (0.75f, 0.0f), (-0.0f, -0.20f));
        Paralax.Repeat = ParalaxRepeat.Both;
        Paralax.RepeatDistance = (48, 0);
    }
}