using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.Hazards;

public class Arrow : Entity
{
    public Arrow()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("Arrow", 32, 16);
        AddCenteredCollisionBox(20, 2);
        AddCandleEnemyShotComponents(10);

        AddDamageTaker(1);

        //// State
        //AddStateManager();
        //var _state = NewState();
        //StateManager.AutomaticStatesList.Add(_state);
    }
}