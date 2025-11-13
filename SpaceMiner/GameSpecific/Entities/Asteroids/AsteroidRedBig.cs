using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Entities.Shared;
using Microsoft.Xna.Framework;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidRedBig : AsteroidRed
{
    public AsteroidRedBig()
    {
        // Sprite
        AddSpriteCenteredOrigin("AsteroidRed2", 48);
        AddCenteredCollisionBox(24);

        // Properties
        AddSpaceMinerEnemyComponents(100, 50);
        AddItemDropper(8, (typeof(OreRed), 2), (typeof(OreGray), 3));

        var duration = 30;
        var damage = 100;
        var size = 128;
        var color = new Color(255, 0, 0, 255);
        AddDeathHandler(new BehaviorCreateBlast(typeof(ResizableBlast), EntityKind.EnemyShot, AlignmentType.Neutral, duration, damage, size, color));
    }
}