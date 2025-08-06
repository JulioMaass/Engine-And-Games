using Engine.ECS.Components.VisualsHandling;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidGreen : Asteroid
{
    public AsteroidGreen()
    {
        // Sprite
        AddSpriteCenteredOrigin("AsteroidGreen2", 48);
        AddSpriteVariation(4, 1);
        AddCenteredCollisionBox(24);
        BloomSource = new BloomSource(this, 0.65f);

        // Properties
        AddSpaceMinerEnemyComponents(100, 50);
        AddItemDropper(12, (typeof(OreGreen), 2), (typeof(OreGray), 3));
        AddRandomMoveSpeed(0.3f, 0.5f);
    }
}