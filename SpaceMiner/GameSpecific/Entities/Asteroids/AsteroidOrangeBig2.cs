using Engine.ECS.Components.VisualsHandling;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidOrangeBig2 : AsteroidOrange
{
    public AsteroidOrangeBig2()
    {
        // Sprite
        AddSpriteCenteredOrigin("AsteroidOrange2", 48);
        Sprite.SpriteSheetOrigin = IntVector2.New(0, 48);
        AddCenteredCollisionBox(24);

        // Properties
        AddSpaceMinerEnemyComponents(400, 50);
        AddItemDropper(8, (typeof(OreOrange), 4), (typeof(OreGray), 5));
    }
}