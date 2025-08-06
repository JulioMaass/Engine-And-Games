using Engine.ECS.Components.VisualsHandling;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidOrange2 : AsteroidOrange
{
    public AsteroidOrange2()
    {
        // Sprite
        Sprite.SpriteSheetOrigin = IntVector2.New(0, 32);

        // Properties
        AddSpaceMinerEnemyComponents(200, 50);
        AddItemDropper(typeof(OreOrange), 10, 8);
    }
}