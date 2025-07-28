using Engine.ECS.Components.MenuHandling;
using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Candle.GameSpecific.Entities.Currency;

public class WaxDrop : Entity
{
    public WaxDrop()
    {
        EntityKind = EntityKind.Item;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSpriteCenteredOrigin("WaxDrop", 16);
        AddCenteredCollisionBox(8);
        AddSolidBehavior(SolidType.NotSolid, SolidInteractionType.StopOnSolids);
        AddGravity();
        AddItemComponents(ResourceType.Wax, 1);


        MenuItem = new MenuItem(this);
        MenuItem.Label = "Wax Drop";
    }
}