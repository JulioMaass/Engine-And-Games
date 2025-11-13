using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Ores;
using SpaceMiner.GameSpecific.Entities.Vfx;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidPurpleBig2 : AsteroidPurple
{
    public AsteroidPurpleBig2()
    {
        // Sprite
        AddSpriteCenteredOrigin("AsteroidPurple2", 48);
        Sprite.SpriteSheetOrigin = IntVector2.New(0, 48);
        AddCenteredCollisionBox(24);

        // Properties
        AddSpaceMinerEnemyComponents(400, 50);
        AddItemDropper(8, (typeof(OrePurple), 4), (typeof(OreGray), 5));

        var deathBehavior = new BehaviorCustom(
            () =>
            {
                var splitRotation = GetRandom.UnseededInt(360000);
                for (var i = 0; i < 8; i++)
                {
                    var angle = GetRandom.UnseededInt(45000) + i * 45000 + splitRotation;
                    var asteroid = EntityManager.CreateEntityAt(typeof(AsteroidPurpleShot), Position.Pixel);
                    asteroid.AddMoveDirection(angle);
                    asteroid.Speed.SetMoveSpeedToCurrentDirection();
                }
            }
        );
        AddDeathHandler(new BehaviorCreateEntity(typeof(VfxDebris)));
        DeathHandler.AddBehavior(deathBehavior);
    }
}