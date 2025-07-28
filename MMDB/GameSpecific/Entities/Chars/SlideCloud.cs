using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Chars;

public class SlideCloud : Entity
{
    public SlideCloud()
    {
        EntityKind = EntityKind.Vfx;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("SlideCloud", 8);
        AddVfxComponents(15);

        // States
        AddStateManager();
        var state = NewState(default, 0, 5, 3);
        StateManager.AutomaticStatesList.Add(state);
    }
}