using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace MMDB.GameSpecific.Entities.Items;

public class HpSmall : Entity
{
    public HpSmall()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("HpSmall", 8);
        AddItemComponents(ResourceType.Hp, 2);

        // States
        AddStateManager();
        var state = NewState(new StateIdleAndFall(), 0, 2, 8);
        StateManager.AutomaticStatesList.Add(state);
    }
}