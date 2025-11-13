using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidGreenBig : AsteroidGreen
{
    public AsteroidGreenBig()
    {
        // Sprite
        AddSpriteCenteredOrigin("AsteroidGreen3", 64);
        AddCenteredCollisionBox(32);

        // Properties
        AddSpaceMinerEnemyComponents(200, 50);
        AddItemDropper(8, (typeof(OreGreen), 2), (typeof(OreGray), 3));
    }
}