using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Enemies;

public class Clock : Entity
{
    public Clock()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("Clock");
        AddCenteredOutlinedCollisionBox();
        AddCandleEnemyComponents(2, 10);
        AddSolidBehavior();

        // States
        AddStateManager();

        // Auto States
        var state = NewState()
            .AddToAutomaticStatesList();

        // Ai Control
        AiControl.AddPermanentBehavior(new BehaviorFacePlayer());
    }
}