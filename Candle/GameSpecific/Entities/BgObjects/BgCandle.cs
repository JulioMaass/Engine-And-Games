using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.Spawning;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Candle.GameSpecific.Entities.BgObjects;

public class BgCandle : Entity
{
    public BgCandle()
    {
        EntityKind = EntityKind.Decoration;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("BgCandle", 24, 24, 12, 12);
        AddCollisionBox(24, 24, 12, 12);

        AddLightSource(IntVector2.Zero, 0, 0);
        SpawnManager.DespawnOnScreenExit = false;

        // States
        AddStateManager();

        // Override States
        var stateBurn = NewState(default, 1, 6, 3)
            .AddStartCondition(new ConditionCollidingWithPlayer())
            .AddStateSettingBehavior(new BehaviorCreateEntity(typeof(FxLightUpCandle), (0, -4)))
            .AddStateSettingBehavior(new BehaviorCustom(() => LightSource.SetRadius(48, 48 + 32)))
            .AddToOverrideStatesList();

        // Auto States
        var stateOff = NewState(default, 0)
            .AddToAutomaticStatesList();
    }
}