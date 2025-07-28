using Engine.ECS.Entities.EntityCreation;

namespace Candle.GameSpecific.Entities.EnemyShots;

public class ShadeSlash : Entity
{
    public ShadeSlash()
    {
        EntityKind = EntityKind.PlayerShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("ShadeSlash", 27, 53);
        AddCandleEnemyShotComponents(10);
        AddSolidBehavior();
        AddCenteredOutlinedCollisionBox();

        // Specific components
        AddFrameCounter(15);

        // State
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}