using Engine.ECS.Entities;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorSetDirectionToPlayer : Behavior
{
    public override void Action()
    {
        Owner.MoveDirection.SetAngleDirectionTo(EntityManager.PlayerEntity);
    }
}
