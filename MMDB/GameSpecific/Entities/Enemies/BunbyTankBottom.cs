using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Enemies;

public class BunbyTankBottom : Entity
{
    public BunbyTankBottom()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("BunbyTankBottom", 28, 12, 13, 5);
        AddMmdbEnemyComponents(3, 4);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);
        AddCollisionBox(24, 10, 12, 4);

        // Enemy specific components
        SpawnManager.AddSpawnBehavior(new BehaviorSnapToFloor());
        AddGravity();

        // Commands
        // AddAiCommand(_ifCantMove, _destroy);

        // Auto States
        // NewState(new WalkAndFall());
    }
}