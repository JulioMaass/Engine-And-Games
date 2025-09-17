using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities;
using Engine.Helpers;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidPurple : Asteroid
{
    public AsteroidPurple()
    {
        // Sprite
        AddSpriteCenteredOrigin("AsteroidPurple", 32);
        AddSpriteVariation(4, 1);
        AddCenteredCollisionBox(16);
        BloomSource = new BloomSource(this, 0.80f);

        // Properties
        AddSpaceMinerEnemyComponents(50, 50);
        AddItemDropper(8, (typeof(OrePurple), 1), (typeof(OreGray), 2));
        AddRandomMoveSpeed(0.4f, 0.6f);

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
        DeathHandler.AddBehavior(deathBehavior);
    }
}