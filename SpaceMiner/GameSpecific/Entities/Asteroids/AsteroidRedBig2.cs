using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Entities.Shared;
using Engine.Types;
using Microsoft.Xna.Framework;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidRedBig2 : AsteroidRed
{
    public AsteroidRedBig2()
    {
        // Sprite
        AddSpriteCenteredOrigin("AsteroidRed2", 48);
        Sprite.SpriteSheetOrigin = IntVector2.New(0, 48);
        AddCenteredCollisionBox(24);

        // Properties
        AddSpaceMinerEnemyComponents(400, 50);
        AddItemDropper(typeof(OreRed), 30, 8);

        var duration = 30;
        var damage = 100;
        var size = 128;
        var color = new Color(255, 0, 0, 255);
        AddDeathHandler(new BehaviorCreateBlast(typeof(ResizableBlast), EntityKind.EnemyShot, AlignmentType.Neutral, duration, damage, size, color));
    }
}