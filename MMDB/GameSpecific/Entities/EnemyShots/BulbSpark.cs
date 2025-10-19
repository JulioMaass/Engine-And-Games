using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.EnemyShots;

public class BulbSpark : Entity
{
    public BulbSpark()
    {
        EntityKind = EntityKind.EnemyShot;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("BulbSpark", 12, 12);
        AddMmdbEnemyShotComponents(2);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // States
        AddStateManager();
        var crawlState = NewState()
            .AddBehavior(new BehaviorAccelerateMoveSpeed());
        crawlState.MovementType = MovementType.Crawling;
        StateManager.AutomaticStatesList.Add(crawlState);
    }
}