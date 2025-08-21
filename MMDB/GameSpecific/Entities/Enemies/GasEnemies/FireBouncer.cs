using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Enemies.GasEnemies;

public class FireBouncer : Entity
{
    public FireBouncer()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("FireBouncer", 16, 16);
        AddMmdbEnemyComponents(2, 3);
        AddCenteredCollisionBox(14, 14);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        // Enemy specific components
        AddMoveDirection();
        AddMoveSpeed(1.95f);

        // States
        AddStateManager();
        // Auto States
        var state = NewState()
            .AddBehavior(new BehaviorBounceTowardsPlayer())
            .AddToAutomaticStatesList();

        // Ai Control
        AiControl.AddPermanentBehavior(new BehaviorFacePlayer());

        AddCustomValueHandler();
        CustomValueHandler.NewValueSetter(0, "direction", () => MoveDirection.Angle = CustomValueHandler.CustomValues[0].Value);
    }
}