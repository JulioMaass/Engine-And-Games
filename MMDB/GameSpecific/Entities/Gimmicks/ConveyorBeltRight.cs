using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using MMDB.GameSpecific.States;
using MMDB.GameSpecific.States.Enemy;

namespace MMDB.GameSpecific.Entities.Gimmicks;

public class ConveyorBeltRight : Entity
{
    public ConveyorBeltRight()
    {
        EntityKind = EntityKind.Gimmick;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("ConveyorBeltRight", 48, 16, 0, 32);
        AddGimmickComponents(0, SolidType.Solid);

        // States
        AddStateManager();
        var state = NewState(new StateCarryRight(), 0, 2, 8);
        StateManager.AutomaticStatesList.Add(state);
    }
}