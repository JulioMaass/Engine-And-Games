using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Mole.GameSpecific.Entities;

public class MoleBombExplosion : Entity
{
    public MoleBombExplosion()
    {
        EntityKind = EntityKind.PlayerShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("MoleBombExplosion", 16);
        AddSolidBehavior();
        AddCenteredCollisionBox(14);
        AddSpeed();

        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(1);
        AddFrameCounter(15);
        AddTileDestructor(Strength.DestroysStrong);

        // State
        AddStateManager();
        var state = NewStateWithTimedPattern(default, (0, 2), (1, 3), (2, 3), (3, 3), (4, 2), (5, 2));
        StateManager.AutomaticStatesList.Add(state);
    }
}