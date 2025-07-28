using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using MMDB.GameSpecific.States;
using MMDB.GameSpecific.States.Enemy;

namespace MMDB.GameSpecific.Entities.Gimmicks;

public class SwingPlat : Entity
{
    public SwingPlat()
    {
        EntityKind = EntityKind.Gimmick;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("SwingPlat", 48, 16, 24, 8);
        AddGimmickComponents(Gravity.DEFAULT_FORCE, SolidType.SolidTop);

        // States
        AddStateManager();
        var state = NewState(new StateSwing());
        StateManager.AutomaticStatesList.Add(state);
    }
}