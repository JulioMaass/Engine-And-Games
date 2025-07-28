using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Enemies.GasEnemies;

public class LampShot : Entity
{
    public LampShot()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("LampShot", 12, 12);
        AddMmdbEnemyShotComponents(3);
        AddSolidBehavior();

        // States
        AddStateManager();
        // Auto States
        var state = NewState()
            .AddToAutomaticStatesList();
    }
}