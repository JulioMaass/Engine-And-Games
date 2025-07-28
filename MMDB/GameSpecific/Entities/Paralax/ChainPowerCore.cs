using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.StageHandling;

namespace MMDB.GameSpecific.Entities.Paralax;

public class ChainPowerCore : Entity
{
    public ChainPowerCore()
    {
        EntityKind = EntityKind.Paralax;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("ChainPowerCore");
        AddParalax(LayerId.LastBackgroundTiles, (0.8f, 0.0f), (0.0f, -0.0f));
        Paralax.Repeat = ParalaxRepeat.None;
        Paralax.RepeatDistance = (0, 0);
    }
}