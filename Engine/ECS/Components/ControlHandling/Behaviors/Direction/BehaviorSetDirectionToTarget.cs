using System.Linq;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Direction;

public class BehaviorSetDirectionToTarget : Behavior
{
    public int PossibleAngles { get; set; }

    public BehaviorSetDirectionToTarget(int possibleAngles = 0)
    {
        PossibleAngles = possibleAngles;
    }

    public override void Action()
    {
        Owner.MoveDirection.SetAngleDirectionTo(Owner.TargetPool.TargetList.FirstOrDefault());
        if (PossibleAngles > 0)
            Owner.MoveDirection.RoundAngle(PossibleAngles);
    }
}
