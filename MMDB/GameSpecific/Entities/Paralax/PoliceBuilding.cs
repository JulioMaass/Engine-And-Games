using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.StageHandling;

namespace MMDB.GameSpecific.Entities.Paralax;

public class PoliceBuilding : Entity
{
    public PoliceBuilding()
    {
        EntityKind = EntityKind.Paralax;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("PoliceBuilding");
        AddParalax(LayerId.LastBackgroundTiles, (0.75f, 1.0f), (0.0f, -0.0f));
        Paralax.Repeat = ParalaxRepeat.X;
        Paralax.RepeatDistance = (0, 0);
    }
}