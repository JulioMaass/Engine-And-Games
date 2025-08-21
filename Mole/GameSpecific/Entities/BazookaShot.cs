using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Facing;
using Engine.ECS.Entities.EntityCreation;

namespace Mole.GameSpecific.Entities;

public class BazookaShot : Entity
{
    public BazookaShot()
    {
        EntityKind = EntityKind.PlayerShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("BazookaShot", 16, 16);
        AddSolidBehavior();
        AddCollisionBox(8, 8, 4, 4);
        AddMoveDirection();

        AddMoveSpeed(2);

        // States
        AddStateManager();
        var state = NewState()
            .AddBehavior(new BehaviorMoveToTopDownFacing());
        state.DirectionFrames = 1;
        StateManager.AutomaticStatesList.Add(state);
    }
}