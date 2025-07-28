using Engine.ECS.Entities.EntityCreation;
using MMDB.GameSpecific.States.Enemy;
using MMDB.GameSpecific.States.Gimmick;

namespace MMDB.GameSpecific.Entities.Gimmicks;

public class LameGate : Entity
{
    public LameGate()
    {
        EntityKind = EntityKind.Gimmick;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteTopLeftOrigin("BossGate", 16, 64);
        Sprite.IsVisible = false;
        AddSolidBehavior();
        AddCollisionBox(16, 64);

        // States
        AddStateManager();
        var state = NewState(new StateCheckToCloseGate());
        StateManager.AutomaticStatesList.Add(state);
    }
}