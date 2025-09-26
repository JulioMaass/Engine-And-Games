using Engine.ECS.Entities.EntityCreation;

namespace SpaceMiner.GameSpecific.Entities.Vfx;

public class VfxBlueTeleport : Entity
{
    public VfxBlueTeleport()
    {
        EntityKind = EntityKind.Vfx;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("BlueTeleport", 32, 32, 16, 16);

        // Vfx
        AddFrameCounter(16);

        // State
        AddStateManager();
        var state = NewState(default, 0, 4, 4);
        StateManager.AutomaticStatesList.Add(state);
    }
}