using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities.EntityCreation;

namespace SpaceMiner.GameSpecific.Entities.Shots;

public class ResizableShot : Entity
{
    public ResizableShot()
    {
        EntityKind = EntityKind.None;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("MegaCircle");
        Sprite.SetColor(255, 127, 0, 255);
        AddSolidBehavior();
        AddCenteredOutlinedCollisionBox();

        AddMoveDirection();
        AddMoveSpeed(4.5f);
        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(10, PiercingType.PierceOnOverkill);

        // State
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}