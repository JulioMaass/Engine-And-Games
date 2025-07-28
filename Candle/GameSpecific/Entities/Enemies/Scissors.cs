using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Enemies;

public class Scissors : Entity
{
    public Scissors()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("Scissors");
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