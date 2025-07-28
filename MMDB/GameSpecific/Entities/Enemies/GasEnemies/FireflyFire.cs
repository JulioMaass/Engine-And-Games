using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Enemies.GasEnemies;

public class FireflyFire : Entity
{
    public FireflyFire()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("FireflyFire", 16);
        AddMmdbEnemyShotComponents(3);
        AddSolidBehavior();
        AddFrameCounter(30);

        // States
        AddStateManager();
        var state = NewState()
            .AddToAutomaticStatesList();
    }
}