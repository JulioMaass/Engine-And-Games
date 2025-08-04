using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.EntityCreation;
using Engine.ECS.Entities.EntityCreation;
using Engine.ECS.Entities.Shared;
using Engine.Helpers;

namespace SpaceMiner.GameSpecific.Entities.Shots;

public class SprayMissile : Entity
{
    public SprayMissile()
    {
        EntityKind = EntityKind.None;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("Missile");
        Sprite.AutoRotation = true;
        AddSolidBehavior();
        AddCenteredCollisionBox(8);

        AddMoveDirection();
        AddMoveSpeed(4.5f);
        AddAlignment(AlignmentType.Friendly);
        AddDamageDealer(10, PiercingType.PierceNone);
        DamageDealer.AddOnHitBehavior(new BehaviorCreateBlast(typeof(ResizableBlast), EntityKind.PlayerShot, AlignmentType.Friendly, 15, 100, 32, CustomColor.PicoBlue));

        // State
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}