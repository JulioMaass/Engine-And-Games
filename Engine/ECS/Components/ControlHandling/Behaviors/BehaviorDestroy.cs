using Engine.ECS.Entities;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorDestroy : Behavior
{
    public override void Action()
    {
        EntityManager.DeleteEntity(Owner);
    }
}
