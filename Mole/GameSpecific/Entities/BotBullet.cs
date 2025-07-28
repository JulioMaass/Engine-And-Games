using Engine.ECS.Entities.EntityCreation;

namespace Mole.GameSpecific.Entities;

public class BotBullet : Entity
{
    public BotBullet()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("BotBullet", 6, 6);
        AddMoleEnemyShotComponents(2);
        AddSolidBehavior();
        AddMoveDirection();

        // States
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}