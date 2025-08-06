using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Entities.Shared;
using Microsoft.Xna.Framework;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidRed : Asteroid
{
    public AsteroidRed()
    {
        // Sprite
        AddSpriteCenteredOrigin("AsteroidRed", 32);
        AddSpriteVariation(4, 1);
        AddCenteredCollisionBox(16);
        BloomSource = new BloomSource(this, 0.8f);

        // Properties
        AddSpaceMinerEnemyComponents(50, 50);
        AddItemDropper(8, (typeof(OreRed), 1), (typeof(OreGray), 2));
        AddRandomMoveSpeed(0.4f, 0.6f);

        var duration = 30;
        var damage = 50;
        var size = 64;
        var color = new Color(255, 0, 0, 255);
        AddDeathHandler(new BehaviorCreateBlast(typeof(ResizableBlast), EntityKind.EnemyShot, AlignmentType.Neutral, duration, damage, size, color));
    }
}