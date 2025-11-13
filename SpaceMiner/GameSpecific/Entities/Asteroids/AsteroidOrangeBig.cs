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
        AddItemDropper(8, (typeof(OreOrange), 2), (typeof(OreGray), 3));
    }
}