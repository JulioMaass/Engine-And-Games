using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.States;

namespace SpaceMiner.GameSpecific.Entities.Ores;

public class OreBlue : Entity
{
    public OreBlue()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("OreBlue", 8);
        AddSpriteVariation(4, 1);
        AddCenteredCollisionBox(8);

        AddItemComponents(ResourceType.OreBlue, 10);
        BloomSource = new BloomSource(this, 0.85f);

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