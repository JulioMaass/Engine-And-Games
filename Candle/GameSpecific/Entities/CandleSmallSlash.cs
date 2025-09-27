using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities;

public class CandleSmallSlash : Entity
{
    public CandleSmallSlash()
    {
        EntityKind = EntityKind.PlayerShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("CandleSmallSlash", 28, 25);
        AddSolidBehavior();
        AddCenteredOutlinedCollisionBox();

        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(1);
        AddFrameCounter(8); // TODO: Check this, 2 animation frames hit for 1 frame

        // State
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}