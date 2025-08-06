using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidPurple2 : AsteroidPurple
{
    public AsteroidPurple2()
    {
        // Sprite
        Sprite.SpriteSheetOrigin = IntVector2.New(0, 32);

        // Properties
        AddSpaceMinerEnemyComponents(200, 50);
        AddItemDropper(typeof(OrePurple), 10, 8);

        var deathBehavior = new BehaviorCustom(
            () =>
            {
                var splitRotation = GetRandom.UnseededInt(360000);
                for (var i = 0; i < 4; i++)
                {
                    var angle = GetRandom.UnseededInt(90000) + i * 90000 + splitRotation;
                    var asteroid = EntityManager.CreateEntityAt(typeof(AsteroidPurpleShot), Position.Pixel);
                    asteroid.AddMoveDirection(angle);
                    asteroid.Speed.SetMoveSpeedToCurrentDirection();
                }
            }
        );
        AddDeathHandler(deathBehavior);
    }
}