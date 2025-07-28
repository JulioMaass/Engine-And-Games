using Engine.ECS.Entities.EntityCreation;

namespace Engine.ECS.Entities.Shared;

public class ResizableBlast : Entity
{
    public ResizableBlast()
    {
        EntityKind = EntityKind.None;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("MegaCircle");
        AddSolidBehavior();
        AddCenteredOutlinedCollisionBox();

        // State
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}