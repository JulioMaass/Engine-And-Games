using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Entities.EntityCreation;

namespace SpaceMiner.GameSpecific.Entities.Shots;

public class DrillMissile : Entity
{
    public DrillMissile()
    {
        EntityKind = EntityKind.None;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("Drill");
        Sprite.AutoRotation = true;
        AddSolidBehavior();
        AddCenteredCollisionBox(28);

        AddMoveDirection();
        AddMoveSpeed(4.5f);
        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(30, PiercingType.PierceAll);

        // State
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}