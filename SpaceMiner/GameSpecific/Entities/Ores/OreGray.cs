using Engine.ECS.Components.VisualsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using SpaceMiner.GameSpecific.States;

namespace SpaceMiner.GameSpecific.Entities.Ores;

public class OreGray : Entity
{
    public OreGray()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("OreGray", 8);
        AddSpriteVariation(4, 1);
        DrawOrder = -1; // Ensure it draws behind other ores
        AddCenteredCollisionBox(8);

        AddItemComponents(ResourceType.OreGray, 10);
        BloomSource = new BloomSource(this, 1.0f);

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