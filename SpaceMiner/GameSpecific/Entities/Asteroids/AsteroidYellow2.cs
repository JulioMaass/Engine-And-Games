using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Ores;
using SpaceMiner.GameSpecific.Entities.Vfx;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidYellow2 : AsteroidYellow
{
    public AsteroidYellow2()
    {
        // Sprite
        Sprite.SpriteSheetOrigin = IntVector2.New(0, 32);

        // Properties
        AddSpaceMinerEnemyComponents(200, 50);
        AddItemDropper(typeof(OreYellow), 10, 8);

        var deathBehavior = new BehaviorCustom(
            () =>
            {
                var splitRotation = GetRandom.UnseededInt(360000);
                for (var i = 0; i < 4; i++)
                {
                    var angle = GetRandom.UnseededInt(90000) + i * 90000 + splitRotation;
                    var asteroid = EntityManager.CreateEntityAt(typeof(AsteroidShard), Position.Pixel);
                    asteroid.AddMoveDirection(angle);
                    asteroid.Speed.SetMoveSpeedToCurrentDirection();
                }
            }
        );
        AddDeathHandler(new BehaviorCreateEntity(typeof(VfxDebris)));
        DeathHandler.AddBehavior(deathBehavior);
    }
}