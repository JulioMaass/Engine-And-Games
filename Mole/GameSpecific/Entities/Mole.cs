using Engine.ECS.Entities.EntityCreation;
using Mole.GameSpecific.States;

namespace Mole.GameSpecific.Entities;

public class Mole : Entity
{
    public Mole()
    {
        EntityKind = EntityKind.Player;
        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("MoleChar", 32, 32, 16, 18);
        AddPlayerComponents();
        Facing = new(this);
        Facing.HasYFacing = true;
        AddSpeed();

        AddItemGetter();

        AddStateManager();
        var stateWalk = NewState(new StateTopDownWalk(), 10, 3, 10);
        var stateIdle = NewState(new StateTopDownIdle());
        StateManager.AutomaticStatesList.Add(stateWalk);
        StateManager.AutomaticStatesList.Add(stateIdle);

        AddCollisionBox(14, 14, 7, 7);
        AddMoveSpeed(1.5f);
    }
}
