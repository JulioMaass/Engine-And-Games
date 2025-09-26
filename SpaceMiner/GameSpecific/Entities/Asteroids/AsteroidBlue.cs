using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Teleporting;
using Engine.ECS.Components.VisualsHandling;
using SpaceMiner.GameSpecific.Entities.Ores;
using SpaceMiner.GameSpecific.Entities.Vfx;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidBlue : Asteroid
{
    public AsteroidBlue()
    {
        // Sprite
        AddSpriteCenteredOrigin("AsteroidBlue", 32);
        AddSpriteVariation(4, 1);
        AddCenteredCollisionBox(16);
        BloomSource = new BloomSource(this, 0.75f);

        // Properties
        AddSpaceMinerEnemyComponents(30, 50);
        AddItemDropper(8, (typeof(OreBlue), 1), (typeof(OreGray), 2));
        AddRandomMoveSpeed(1.5f, 2.0f);

        // States
        AddStateManager();
        // Auto States
        var state = NewState()
            .AddBehavior(new BehaviorWarp(this, 2, typeof(VfxBlueTeleport)))
            .AddToAutomaticStatesList();
    }
}