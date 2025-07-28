using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Components.ControlHandling.States;

namespace Candle.GameSpecific.Entities.Gimmicks;

public class FallBridge : Entity
{
    public FallBridge()
    {
        EntityKind = EntityKind.Gimmick;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("FallBridge", 16, 16, 8, 8);
        AddFullCollisionBox();
        AddSolidBehavior(SolidType.SolidTop);
        AddGravity(0.15f);

        Gravity.IsAffectedByGravity = false;

        // States
        AddStateManager();
        var stateTrigger = NewState(new StateFallOnStep(15))
        .AddToAutomaticStatesList();
        var state = NewState()
        .AddToAutomaticStatesList();
    }
}