using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Direction;
using Engine.ECS.Components.ControlHandling.Behaviors.Direction;
using Engine.ECS.Components.ControlHandling.Behaviors.Facing;
using Engine.ECS.Components.ControlHandling.Behaviors.Targeting;
using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Enemies;

public class Telly : Entity
{
    public Telly()
    {
        EntityKind = EntityKind.Enemy;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("Telly", 16);
        AddMmdbEnemyComponents(1, 2);
        AddSolidBehavior();

        // Enemy specific components
        AddMoveDirection();
        AddMoveSpeed(1.0f);
        AddTurnSpeed(2000);

        // States
        AddStateManager();
        // Auto States
        var state = NewState(default, 0, 4, 12)
            .AddStateSettingBehavior(new BehaviorTargetNearestEntity(AlignmentType.Friendly, EntityKind.Player))
            .AddStateSettingBehavior(new BehaviorFacePlayer())
            .AddStateSettingBehavior(new BehaviorSetDirectionToTarget(2))
            .AddBehavior(new BehaviorTargetNearestEntity(AlignmentType.Friendly, EntityKind.Player))
            .AddBehavior(new BehaviorTurnTowardsTarget())
            .AddBehavior(new BehaviorMoveToCurrentDirection())
            .AddToAutomaticStatesList();
    }
}