using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Enemies;

public class HeliBomb : Entity
{
    public HeliBomb()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("HeliBomb");
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