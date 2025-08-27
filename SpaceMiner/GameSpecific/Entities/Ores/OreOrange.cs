using Engine.ECS.Components.CombatHandling;
using Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Direction;
using Engine.ECS.Components.ControlHandling.Behaviors.Direction;
using Engine.ECS.Components.ControlHandling.Behaviors.Speed;
using Engine.ECS.Components.ControlHandling.Behaviors.Targeting;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Ores;

public class OreOrange : Entity
{
    public OreOrange()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("OreOrange", 8);
        AddSpriteVariation(4, 1);
        AddCenteredCollisionBox(8);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        AddItemComponents(ResourceType.OreOrange, 10);
        BloomSource = new BloomSource(this, 0.75f);

        //MenuItem = new MenuItem(this);
        //MenuItem.Label = "Wax Ball";

        AddMoveSpeed(0.5f);
        AddMoveDirection(90000);

        // States
        AddStateManager();
        // Auto States
        var stateHomeToPlayer = NewState()
            .AddStartCondition(new ConditionDistanceToPlayerLessThan(32))
            .AddBehavior(new BehaviorMoveToCurrentDirection())
            .AddBehavior(new BehaviorTargetNearestEntity(AlignmentType.Friendly, EntityKind.Player))
            .AddBehavior(new BehaviorSetDirectionToTarget())
            .AddBehavior(new BehaviorChangeMoveSpeed(3f))
            .AddToAutomaticStatesList();
        var stateFall = NewState()
            .AddBehavior(new BehaviorMoveToCurrentDirection())
            .AddToAutomaticStatesList();
    }
}