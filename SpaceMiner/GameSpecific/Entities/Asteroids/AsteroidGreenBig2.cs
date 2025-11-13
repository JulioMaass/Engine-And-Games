using Engine.ECS.Components.VisualsHandling;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidGreenBig2 : AsteroidGreen
{
    public AsteroidGreenBig2()
    {
        // Sprite
        AddSpriteCenteredOrigin("AsteroidGreen3", 64);
        Sprite.SpriteSheetOrigin = IntVector2.New(0, 64);
        AddCenteredCollisionBox(32);

        // Properties
        AddSpaceMinerEnemyComponents(800, 50);
        AddItemDropper(8, (typeof(OreGreen), 4), (typeof(OreGray), 5));
    }
}