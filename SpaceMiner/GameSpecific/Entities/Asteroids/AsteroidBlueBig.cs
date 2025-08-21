using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Teleporting;
using SpaceMiner.GameSpecific.Entities.Ores;

namespace SpaceMiner.GameSpecific.Entities.Asteroids;

public class AsteroidBlueBig : AsteroidBlue
{
    public AsteroidBlueBig()
    {
        // Sprite
        AddSpriteCenteredOrigin("AsteroidBlue2", 48);
        AddCenteredCollisionBox(24);

        // Properties
        AddSpaceMinerEnemyComponents(60, 50);
        AddItemDropper(12, (typeof(OreBlue), 3), (typeof(OreGray), 5));

        // States
        AddStateManager();
        // Auto States
        var state = NewState()
            .AddBehavior(new BehaviorWarp(this, 4))
            .AddToAutomaticStatesList();
    }
}