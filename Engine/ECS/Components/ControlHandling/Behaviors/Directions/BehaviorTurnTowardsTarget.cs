using Engine.ECS.Components.PositionHandling;
using System.Linq;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Directions;

public class BehaviorTurnTowardsTarget : Behavior
{
    public Direction Direction { get; set; }

    public BehaviorTurnTowardsTarget(Direction direction)
    {
        Direction = direction;
    }

    public override void Action()
    {
        Direction.TurnTowards(Owner.TargetPool.TargetList.FirstOrDefault());
    }
}
