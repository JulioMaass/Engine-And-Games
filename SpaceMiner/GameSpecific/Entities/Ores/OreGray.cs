using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Ores;

public class OreGray : Entity
{
    public OreGray()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteFullImageCenteredOrigin("OreGray");
        AddCenteredCollisionBox(8);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);

        AddItemComponents(ResourceType.OreGray, 1);

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
            .AddBehavior(new BehaviorSetDirectionToPlayer())
            .AddBehavior(new BehaviorChangeMoveSpeed(3f))
            .AddToAutomaticStatesList();
        var stateFall = NewState()
            .AddBehavior(new BehaviorMoveToCurrentDirection())
            .AddToAutomaticStatesList();
    }
}