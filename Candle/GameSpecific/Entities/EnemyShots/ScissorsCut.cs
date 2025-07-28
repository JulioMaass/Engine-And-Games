using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.EnemyShots;

public class ScissorsCut : Entity
{
    public ScissorsCut()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("ScissorsCut");
        AddCandleEnemyShotComponents(10);
        AddSolidBehavior();
        AddMoveDirection();

        // States
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}