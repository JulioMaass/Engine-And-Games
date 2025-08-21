using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Direction;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace SpaceMiner.GameSpecific.Entities.Background;

public class Star : Entity
{
    public Star()
    {
        EntityKind = EntityKind.Paralax;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("Star", 8);
        AddSpriteVariation(4, 1);
        AddCenteredCollisionBox(8);

        var speed = (10 - Sprite.VariationOffset) / 50f;
        AddMoveSpeed(speed);
        AddMoveDirection(90000);

        BloomSource = new BloomSource(this, 1.0f);

        // States
        AddStateManager();
        // Auto States
        var stateFall = NewState()
            .AddBehavior(new BehaviorMoveToCurrentDirection())
            .AddToAutomaticStatesList();
    }
}