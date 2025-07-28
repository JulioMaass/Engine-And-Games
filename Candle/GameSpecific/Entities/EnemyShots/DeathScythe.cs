using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.EnemyShots;

public class DeathScythe : Entity
{
    public DeathScythe()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("DeathScythe");
        AddCandleEnemyShotComponents(10);
        AddSolidBehavior();
        AddMoveDirection();

        // States
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}