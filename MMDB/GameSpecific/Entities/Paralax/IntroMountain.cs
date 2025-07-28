using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.StageHandling;

namespace MMDB.GameSpecific.Entities.Paralax;

public class IntroMountain : Entity
{
    public IntroMountain()
    {
        EntityKind = EntityKind.Paralax;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("IntroMountain");
        AddParalax(LayerId.LastBackgroundTiles, (0.7f, 0.7f), (0.0f, -0.0f));
        Paralax.Repeat = ParalaxRepeat.X;
        Paralax.RepeatDistance = (0, 0);
    }
}