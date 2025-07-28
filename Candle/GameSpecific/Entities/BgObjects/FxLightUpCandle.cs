using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.BgObjects;

public class FxLightUpCandle : Entity
{
    public FxLightUpCandle()
    {
        EntityKind = EntityKind.DecorationVfx;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("FxLightUpCandle", 16, 16, 8, 8);
        AddVfxComponents(9);

        // States
        AddStateManager();
        var state = NewState(default, 0, 3, 3)
            .AddToAutomaticStatesList();
    }
}