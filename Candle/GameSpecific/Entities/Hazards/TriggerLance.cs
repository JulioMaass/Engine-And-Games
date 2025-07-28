using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PositionHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Hazards;

public class TriggerLance : Entity
{
    public TriggerLance()
    {
        EntityKind = EntityKind.Gimmick;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("FloorLance", 16, 48, 8, 40);
        AddCollisionBox(4, 32, 2, 24);
        AddCandleGimmickComponents();

        AddAlignment(AlignmentType.Hostile);
        AddDamageDealer(10);
        AddDamageTaker(1);

        // States
        AddStateManager();
        var stateAttack = NewState(default, 1)
            .AddKeepCondition(new ConditionFrameSmaller(30))
            .AddStateSettingBehavior(new BehaviorChangeBodyType(BodyType.Invincible))
            .AddToAutomaticStatesList();
        var stateTriggered = NewState()
            .AddToAutomaticStatesList();
        var stateHidden = NewState()
            .AddStateSettingBehavior(new BehaviorChangeBodyType(BodyType.Bypass))
            .AddToAutomaticStatesList();
        stateTriggered
            .AddStartCondition(new ConditionState(stateHidden))
            .AddStartCondition(new ConditionCollidingWithPlayer());
        stateAttack
            .AddStartCondition(new ConditionStateFrame(stateTriggered, 15));
    }
}