using Engine.ECS.Entities.EntityCreation;
using MMDB.GameSpecific.States.Enemy;

namespace MMDB.GameSpecific.Entities.Enemies;

public class Telly : Entity
{
    public Telly()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("Telly", 16);
        AddMmdbEnemyComponents(1, 2);
        AddSolidBehavior();

        // Enemy specific components
        AddMoveDirection();
        AddMoveSpeed(1.0f);
        AddTurnSpeed(2);

        // States
        AddStateManager();
        // Auto States
        var state = NewState(new StateEnemyTurnAngleAndMove(), 0, 4, 12)
            .AddToAutomaticStatesList();
    }
}