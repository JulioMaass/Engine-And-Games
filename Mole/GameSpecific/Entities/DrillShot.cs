using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Mole.GameSpecific.Entities;

public class DrillShot : Entity
{
    public DrillShot()
    {
        EntityKind = EntityKind.PlayerShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("DrillShot", 16, 16);
        AddSolidBehavior();
        AddCollisionBox(8, 8, 4, 4);
        AddMoveDirection();

        // Components
        AddMoveSpeed(2);
        AddTileDestructor(Strength.DestroysStrong);

        // States
        AddStateManager();
        var state = NewState()
            .AddBehavior(new BehaviorMoveToTopDownFacing());
        state.DirectionFrames = 1;
        StateManager.AutomaticStatesList.Add(state);
    }
}