using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Enemies;

public class BunbyTankTop : Entity
{
    public BunbyTankTop()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("BunbyTankTop", 46, 22, 19, 8);
        AddMmdbEnemyComponents(3, 4);
        AddSolidBehavior();
        AddCollisionBox(22, 16, 11, 5);

        // Cycle between  Move and Shoot
        // Cycle movement (Use a variable inside AiControl to determine direction? Make an 8 state cycle?)
    }
}