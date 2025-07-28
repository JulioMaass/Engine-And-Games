using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Hazards;

public class IceCeilingSpikes : Entity
{
    public IceCeilingSpikes()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("IceCeilingSpikes", 16, 16);
        AddCenteredCollisionBox(16, 16);
        AddCandleEnemyShotComponents(10);

        AddGravity();
        Gravity.IsAffectedByGravity = false;

        // States
        AddStateManager();
        var stateTriggered = NewState()
            .AddStartCondition(new ConditionPlayerXDistanceRange(0, 32))
            .AddStateSettingBehavior(new BehaviorCustom(() => Gravity.IsAffectedByGravity = true))
            .AddToAutomaticStatesList();
        var stateIdle = NewState()
            .AddToAutomaticStatesList();
    }
}