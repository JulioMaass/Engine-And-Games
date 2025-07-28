using Engine.ECS.Entities.EntityCreation;
using ShooterGame.GameSpecific;

namespace ShooterGame.GameSpecific.Entities;

public class ShooterEnemyShot : Entity
{
    public ShooterEnemyShot()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("ShooterEnemyShot");
        AddShooterEnemyShotComponents(1);
        AddSolidBehavior();
        AddMoveDirection();

        // States
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}