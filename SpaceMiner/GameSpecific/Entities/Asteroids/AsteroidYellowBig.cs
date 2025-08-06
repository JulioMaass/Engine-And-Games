using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Entities;
using Engine.Helpers;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidYellowBig : AsteroidYellow
{
    public AsteroidYellowBig()
    {
        // Sprite
        AddSpriteCenteredOrigin("AsteroidYellow2", 48);
        AddCenteredCollisionBox(24);

        // Properties
        AddSpaceMinerEnemyComponents(100, 50);
        ItemDropper = null;

        var deathBehavior = new BehaviorCustom(
            () =>
            {
                var splitRotation = GetRandom.UnseededInt(360000);
                for (var i = 0; i < 4; i++)
                {
                    var angle = GetRandom.UnseededInt(90000) + i * 90000 + splitRotation;
                    var asteroid = EntityManager.CreateEntityAt(typeof(AsteroidYellow), Position.Pixel);
                    asteroid.AddMoveDirection(angle);
                    asteroid.Speed.SetMoveSpeedToCurrentDirection();
                }
            }
        );
        AddDeathHandler(deathBehavior);
    }
}