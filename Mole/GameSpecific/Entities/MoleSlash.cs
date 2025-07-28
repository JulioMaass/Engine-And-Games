using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Mole.GameSpecific.Entities;

public class MoleSlash : Entity
{
    public MoleSlash()
    {
        EntityKind = EntityKind.PlayerShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("MoleSlashAnimated", 16);
        AddSolidBehavior();
        AddCenteredCollisionBox(14);
        AddSpeed();

        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(1);
        AddFrameCounter(10);
        AddTileDestructor(Strength.DestroysWeak);

        // State
        AddStateManager();
        var state = NewStateWithTimedPattern(default, (0, 1), (1, 2), (2, 3), (3, 2), (4, 1), (5, 1));
        StateManager.AutomaticStatesList.Add(state);
    }
}