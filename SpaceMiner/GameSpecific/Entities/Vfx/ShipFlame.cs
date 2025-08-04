using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace SpaceMiner.GameSpecific.Entities.Vfx;

public class ShipFlame : Entity
{
    public ShipFlame()
    {
        EntityKind = EntityKind.DecorationVfx;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("CircleFill8To1", 8, 8, 4, 4);
        AddVfxComponents(4);

        AddSpeed(0f, 2f);
        BloomSource = new BloomSource(this, 0.65f);

        // States
        AddStateManager();
        var state = NewStateWithTimedPattern(default, (0, 1), (2, 1), (4, 1), (6, 1))
            .AddToAutomaticStatesList();
    }
}