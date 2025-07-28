using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using Mole.GameSpecific.States;

namespace Mole.GameSpecific.Entities;

public class EnemyWalker : Entity
{
    public EnemyWalker()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("EnemyWalker", 16);
        AddMoleEnemyComponents(3, 1);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components
        AddMoveDirection();

        // States // TODO: Walk randomly when player is unreachable and alternate between walking and idle
        AddStateManager();
        var state = NewState(new StateWalkToPlayer(1.0f));
        StateManager.AutomaticStatesList.Add(state);
    }
}