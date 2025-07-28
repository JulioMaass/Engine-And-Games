using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Mole.GameSpecific.Entities;

public class BombItem : Entity
{
    public BombItem()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("BombItem", 12);
        AddCenteredCollisionBox(12);
        AddSpeed();
        AddResourceItemStats(ResourceType.Bombs, 1);

        // State
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}