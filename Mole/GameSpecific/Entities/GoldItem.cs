using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Mole.GameSpecific.Entities;

public class GoldItem : Entity
{
    public GoldItem()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("GoldItem", 8);
        AddCenteredCollisionBox(8);
        AddSpeed();
        AddResourceItemStats(ResourceType.Gold, 1);

        // State
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}