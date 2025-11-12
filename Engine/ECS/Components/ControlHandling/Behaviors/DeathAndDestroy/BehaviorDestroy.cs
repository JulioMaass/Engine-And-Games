using Engine.ECS.Entities;

namespace Engine.ECS.Components.ControlHandling.Behaviors.DeathAndDestroy;

public class BehaviorDestroy : Behavior
{
    public override void Action()
    {
        EntityManager.MarkEntityForDeletion(Owner);
        Owner.DeathHandler?.RunDeathProcess();
    }
}
