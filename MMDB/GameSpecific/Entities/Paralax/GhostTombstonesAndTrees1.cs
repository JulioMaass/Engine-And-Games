using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.StageHandling;

namespace MMDB.GameSpecific.Entities.Paralax;

public class GhostTombstonesAndTrees1 : Entity
{
    public GhostTombstonesAndTrees1()
    {
        EntityKind = EntityKind.Paralax;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("GhostTombstonesAndTrees1");
        AddParalax(LayerId.LastBackgroundTiles, (0.5f, 1.0f), (0.0f, -0.0f));
        Paralax.Repeat = ParalaxRepeat.X;
        Paralax.RepeatDistance = (0, 0);
    }
}