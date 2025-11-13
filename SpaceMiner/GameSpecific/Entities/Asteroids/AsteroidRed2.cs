using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Entities.Shared;
using Engine.Types;
using Microsoft.Xna.Framework;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidRed2 : AsteroidRed
{
    public AsteroidRed2()
    {
        // Sprite
        Sprite.SpriteSheetOrigin = IntVector2.New(0, 32);

        // Properties
        AddSpaceMinerEnemyComponents(200, 50);
        AddItemDropper(8, (typeof(OreRed), 3), (typeof(OreGray), 4));

        var duration = 30;
        var damage = 50;
        var size = 64;
        var color = new Color(255, 0, 0, 255);
        AddDeathHandler(new BehaviorCreateBlast(typeof(ResizableBlast), EntityKind.EnemyShot, AlignmentType.Neutral, duration, damage, size, color));
    }
}