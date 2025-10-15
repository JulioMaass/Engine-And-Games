using Engine.ECS.Components.ControlHandling.Behaviors.CollisionBox;
using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Components.ControlHandling.Conditions;

namespace MMDB.GameSpecific.Entities.Explosions;

public class ExplosionBig : Entity
{
    public ExplosionBig()
    {
        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("SmallExplosion", 24);
        AddCollisionBox(32, 32, 16, 16);
        AddFrameCounter(15);

        // States
        AddStateManager();
        // ReSharper disable once ComplexConditionExpression
        var state = NewState(default, 0, 3, 5)
            .AddBehaviorWithConditions(new BehaviorChangeBodyType(Engine.ECS.Components.PositionHandling.BodyType.Bypass), new ConditionFrameEqual(6))
            .AddBehaviorWithConditions(new BehaviorCreateEntity(typeof(ExplosionSmall), (12, -12)), new ConditionFrameEqual(3))
            .AddBehaviorWithConditions(new BehaviorCreateEntity(typeof(ExplosionSmall), (-12, 12)), new ConditionFrameEqual(3))
            .AddBehaviorWithConditions(new BehaviorCreateEntity(typeof(ExplosionSmall), (16, 0)), new ConditionFrameEqual(6))
            .AddBehaviorWithConditions(new BehaviorCreateEntity(typeof(ExplosionSmall), (-16, 0)), new ConditionFrameEqual(6))
            .AddBehaviorWithConditions(new BehaviorCreateEntity(typeof(ExplosionSmall), (12, 12)), new ConditionFrameEqual(9))
            .AddBehaviorWithConditions(new BehaviorCreateEntity(typeof(ExplosionSmall), (-12, -12)), new ConditionFrameEqual(9))
            .AddBehaviorWithConditions(new BehaviorCreateEntity(typeof(ExplosionSmall), (0, 16)), new ConditionFrameEqual(12))
            .AddBehaviorWithConditions(new BehaviorCreateEntity(typeof(ExplosionSmall), (0, -16)), new ConditionFrameEqual(12));
        StateManager.AutomaticStatesList.Add(state);
    }
}