using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.EnemyShots;

public class Explosion48 : Entity
{
    public Explosion48()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("Explosion48");
        AddCandleEnemyShotComponents(10);
        AddSolidBehavior();
        AddMoveDirection();

        // States
        AddStateManager();
        var state = NewState()
            .AddBehaviorWithConditions(new BehaviorDestroy(), new ConditionFrame(15, ComparisonType.Equal)); ;
        StateManager.AutomaticStatesList.Add(state);


    }
}