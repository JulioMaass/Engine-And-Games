using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.StageHandling;

namespace MMDB.GameSpecific.Entities.Paralax;

public class IntroCloud : Entity
{
    public IntroCloud()
    {
        EntityKind = EntityKind.Paralax;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("ParalaxCloud", 96, 32);
        AddParalax(LayerId.LastBackgroundTiles, (0.7f, 0.1f), (0.25f, -0.0f));
        Paralax.Repeat = ParalaxRepeat.X;
        Paralax.RepeatDistance = (384 - 128, 32);
    }
}