using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.EnemyShots;

public class Rune : Entity
{
    public Rune()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("Rune");
        AddCandleEnemyShotComponents(10);
        AddSolidBehavior();
        AddMoveDirection();

        // States
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}