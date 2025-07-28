using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.EnemyShots;

public class DarkMageSkull : Entity
{
    public DarkMageSkull()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("DarkMageSkull");
        AddCandleEnemyShotComponents(10);
        AddSolidBehavior();
        AddMoveDirection();

        // States
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}