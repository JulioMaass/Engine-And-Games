using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Entities.EntityCreation;
using SpaceMiner.GameSpecific.Entities.Ores;
using SpaceMiner.GameSpecific.Entities.Vfx;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class Asteroid : Entity
{
    public Asteroid()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("Asteroid");
        AddCenteredOutlinedCollisionBox();
        AddSpaceMinerEnemyComponents(30, 50);
        AddSolidBehavior();
        AddItemDropper(typeof(OreGray), 3, 8);

        AddRandomMoveSpeed(0.4f, 0.6f);
        Speed.MaxSpeed = 8f;
        AddMoveDirection();
        AddDeathHandler(new BehaviorCreateEntity(typeof(VfxDebris)));

        // States
        AddStateManager();
        // Auto States
        var state = NewState()
            .AddToAutomaticStatesList();
    }
}