using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities;

public class CandleLance : Entity
{
    public CandleLance()
    {
        EntityKind = EntityKind.PlayerShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("CandleLanceThrust", 70, 18);
        AddSolidBehavior();
        AddCenteredOutlinedCollisionBox();

        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(1);
        DamageDealer.HitType = HitType.HitOnce;
        AddFrameCounter(8); // TODO: Check this, 2 animation frames hit for 1 frame

        // State
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}