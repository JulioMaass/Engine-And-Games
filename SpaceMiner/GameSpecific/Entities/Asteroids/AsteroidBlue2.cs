using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Teleporting;
using Engine.ECS.Components.VisualsHandling;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Ores;
using SpaceMiner.GameSpecific.Entities.Vfx;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidBlue2 : AsteroidBlue
{
    public AsteroidBlue2()
    {
        // Sprite
        Sprite.SpriteSheetOrigin = IntVector2.New(0, 32);

        // Properties
        AddSpaceMinerEnemyComponents(120, 50);
        AddItemDropper(8, (typeof(OreBlue), 3), (typeof(OreGray), 4));

        // States
        AddStateManager();
        // Auto States
        var state = NewState()
            .AddBehavior(new BehaviorWarp(this, 6, typeof(VfxBlueTeleport)))
            .AddToAutomaticStatesList();
    }
}