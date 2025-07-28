using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.BgObjects;

public class CandleFlame : Entity
{
    public CandleFlame()
    {
        EntityKind = EntityKind.Vfx;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("CircleFill8To1", 8, 8, 4, 4);
        AddVfxComponents(4);

        AddSpeed(0f, -2f);

        // States
        AddStateManager();
        var state = NewStateWithTimedPattern(default, (0, 1), (2, 1), (4, 1), (6, 1))
            .AddToAutomaticStatesList();
    }
}