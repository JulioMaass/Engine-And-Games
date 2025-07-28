using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using MMDB.GameSpecific.States;

namespace MMDB.GameSpecific.Entities.Gimmicks;

public class TriggerPlatDown : Entity
{
    public TriggerPlatDown()
    {
        EntityKind = EntityKind.Gimmick;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteTopLeftOrigin("DebugTiles", 32, 16, 32, 16);
        AddGimmickComponents(Gravity.DEFAULT_FORCE, SolidType.Solid);

        Gravity.IsAffectedByGravity = false;

        // States
        AddStateManager();
        var stateTrigger = NewState(new StateFallOnStep());
        StateManager.AutomaticStatesList.Add(stateTrigger);
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}