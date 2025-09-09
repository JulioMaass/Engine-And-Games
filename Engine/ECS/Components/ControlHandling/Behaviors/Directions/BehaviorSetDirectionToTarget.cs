using Engine.ECS.Components.PositionHandling;
using System.Linq;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Directions;

public class BehaviorSetDirectionToTarget : Behavior
{
    public Direction Direction { get; set; }
    public int PossibleAngles { get; set; }

    public BehaviorSetDirectionToTarget(Direction direction, int possibleAngles = 0)
    {
        PossibleAngles = possibleAngles;
        Direction = direction;
    }

    public override void Action()
    {
        Direction.SetAngleDirectionTo(Owner.TargetPool.TargetList.FirstOrDefault());
        if (PossibleAngles > 0)
            Direction.RoundAngle(PossibleAngles);
    }
}
