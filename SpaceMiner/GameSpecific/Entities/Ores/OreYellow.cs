using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.States;

namespace SpaceMiner.GameSpecific.Entities.Ores;

public class OreYellow : Entity
{
    public OreYellow()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("OreYellow", 8);
        AddSpriteVariation(4, 1);
        AddCenteredCollisionBox(8);

        AddItemComponents(ResourceType.OreYellow, 10);
        BloomSource = new BloomSource(this, 0.80f);

        //MenuItem = new MenuItem(this);
        //MenuItem.Label = "Wax Ball";

        AddMoveSpeed(0.5f);
        AddMoveDirection(90000);

        // States
        AddStateManager();
        // Auto States
        var state = NewState(new StateOre());
        StateManager.AutomaticStatesList.Add(state);
    }
}