using System.Linq;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Direction;

public class BehaviorTurnTowardsTarget : Behavior
{
    public override void Action()
    {
        Owner.MoveDirection.TurnTowards(Owner.TargetPool.TargetList.FirstOrDefault());
    }
}
