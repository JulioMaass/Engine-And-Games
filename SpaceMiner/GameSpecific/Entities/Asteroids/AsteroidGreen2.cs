using Engine.ECS.Components.VisualsHandling;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidGreen2 : AsteroidGreen
{
    public AsteroidGreen2()
    {
        // Sprite
        Sprite.SpriteSheetOrigin = IntVector2.New(0, 48);

        // Properties
        AddSpaceMinerEnemyComponents(400, 50);
        AddItemDropper(typeof(OreGreen), 20, 8);
    }
}