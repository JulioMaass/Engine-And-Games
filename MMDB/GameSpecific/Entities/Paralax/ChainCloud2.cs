using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.StageHandling;

namespace MMDB.GameSpecific.Entities.Paralax;

public class ChainCloud2 : Entity
{
    public ChainCloud2()
    {
        EntityKind = EntityKind.Paralax;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("ChainCloud2");
        AddParalax(LayerId.LastBackgroundTiles, (0.5f, 0.0f), (-0.05f, -0.0f));
        Paralax.Repeat = ParalaxRepeat.X;
        Paralax.RepeatDistance = (384, 0);
    }
}