using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities;

public class CandleArrow : Entity
{
    public CandleArrow()
    {
        EntityKind = EntityKind.PlayerShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("CandleArrow", 32, 16);
        AddSolidBehavior();
        AddCenteredOutlinedCollisionBox();

        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(1, PiercingType.PierceNone);
        AddMoveSpeed(4.0f);

        // State
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}