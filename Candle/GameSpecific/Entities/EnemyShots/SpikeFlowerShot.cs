using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.EnemyShots;

public class SpikeFlowerShot : Entity
{
    public SpikeFlowerShot()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("SpikeFlowerShot", 8, 8, 4, 4);
        Sprite.DirectionOffset = (8, 1);
        AddCandleEnemyShotComponents(10, 1);
        AddSolidBehavior();
        AddMoveDirection();

        // States
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}