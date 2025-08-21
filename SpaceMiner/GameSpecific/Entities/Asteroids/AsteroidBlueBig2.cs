using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Teleporting;
using Engine.ECS.Components.VisualsHandling;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidBlueBig2 : AsteroidBlue
{
    public AsteroidBlueBig2()
    {
        // Sprite
        AddSpriteCenteredOrigin("AsteroidBlue2", 48);
        Sprite.SpriteSheetOrigin = IntVector2.New(0, 48);
        AddCenteredCollisionBox(24);

        // Properties
        AddSpaceMinerEnemyComponents(240, 50);
        AddItemDropper(typeof(OreBlue), 30, 8);

        // States
        AddStateManager();
        // Auto States
        var state = NewState()
            .AddBehavior(new BehaviorWarp(this, 8))
            .AddToAutomaticStatesList();
    }
}