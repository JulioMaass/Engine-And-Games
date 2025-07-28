using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.Death;
using Engine.ECS.Entities.EntityCreation;
using System.Collections.Generic;
using Mole.GameSpecific.Behaviors;

namespace Mole.GameSpecific.Entities;

public class MoleBomb : Entity
{
    public MoleBomb()
    {
        EntityKind = EntityKind.PlayerShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("MoleBomb", 16);
        AddCenteredCollisionBox(14);
        AddSpeed();

        AddAlignment(AlignmentType.Friendly);
        AddFrameCounter(120, true);

        // State
        AddStateManager();
        var state = NewState();
        state.SpritePattern = new List<int> {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            1, 1, 0, 0, 1, 1, 0, 0, 1, 1,
            0, 0, 1, 1, 0, 0, 1, 1, 0, 0,
            1, 1, 0, 0, 1, 1, 0, 0, 1, 1,
            2, 2, 1, 1, 2, 2, 1, 1, 2, 2,
            1, 1, 2, 2, 1, 1, 2, 2, 1, 1,
            2, 2, 3, 3, 2, 2, 3, 3, 2, 2,
        };
        StateManager.AutomaticStatesList.Add(state);

        AddDeathHandler(new BehaviorMoleBombDeath());
    }
}