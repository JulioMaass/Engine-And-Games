using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities.EntityCreation;
using ShooterGame.GameSpecific;

namespace ShooterGame.GameSpecific.Entities;

public class ShooterPlayerShot : Entity
{
    public ShooterPlayerShot()
    {
        EntityKind = EntityKind.PlayerShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("ShooterPlayerShot");
        AddSolidBehavior();
        AddCenteredOutlinedCollisionBox();

        AddMoveDirection();
        AddMoveSpeed(4.5f);
        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(1, PiercingType.PierceNone);

        // State
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}