using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PositionHandling;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Hazards;

public class TimedLance : Entity
{
    public TimedLance()
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
        // Auto States
        var stateHidden = NewState()
            .AddStateSettingBehavior(new BehaviorChangeBodyType(BodyType.Bypass))
            .AddToAutomaticStatesList();
        // Command States
        var stateAttack = NewState(default, 1)
            .AddKeepCondition(new ConditionFrameSmaller(60))
            .AddStateSettingBehavior(new BehaviorChangeBodyType(BodyType.Invincible));

        //Ai Control
        AiControl = new(this);
        AiControl.SetConditionsToTriggerDecision(new ConditionStateFrame(stateHidden, 60));
        AiControl.AddSingleStatePool(stateAttack);
    }
}