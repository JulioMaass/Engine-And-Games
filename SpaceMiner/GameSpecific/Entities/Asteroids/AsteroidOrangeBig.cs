using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidOrangeBig : AsteroidOrange
{
    public AsteroidOrangeBig()
    {
        // Sprite
        AddSpriteCenteredOrigin("AsteroidOrange2", 48);
        AddCenteredCollisionBox(24);

        // Properties
        AddSpaceMinerEnemyComponents(100, 50);
        AddItemDropper(12, (typeof(OreOrange), 3), (typeof(OreGray), 5));
    }
}