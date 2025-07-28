using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;

namespace MMDB.GameSpecific.Entities.Gimmicks;

public class DestructibleBlock : Entity
{
    public DestructibleBlock()
    {
        EntityKind = EntityKind.Gimmick;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("DestructibleBlock", 16, 16, 8, 8);
        AddGimmickComponents(Gravity.DEFAULT_FORCE, SolidType.Solid, SolidInteractionType.StopOnSolids);

        // States
        AddStateManager();
        var state = NewState();
        StateManager.AutomaticStatesList.Add(state);
    }
}