using Engine.ECS.Components.LinkedEntities;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidGreenShield : Asteroid
{
    public AsteroidGreenShield()
    {
        // Sprite
        AddSpriteCenteredOrigin("AsteroidGreen", 32);
        AddSpriteVariation(4, 1);
        AddCenteredCollisionBox(24);
        BloomSource = new BloomSource(this, 0.65f);

        // Properties
        AddSpaceMinerEnemyComponents(50, 50);
        AddItemDropper(12, (typeof(OreGreen), 2), (typeof(OreGray), 3));
        AddRandomMoveSpeed(0.3f, 0.5f);

        // Shield
        var shield = EntityManager.CreateEntity(typeof(Shield64));
        LinkedEntitiesManager = new LinkedEntitiesManager(this);
        LinkedEntitiesManager.LinkShield(shield);
    }
}