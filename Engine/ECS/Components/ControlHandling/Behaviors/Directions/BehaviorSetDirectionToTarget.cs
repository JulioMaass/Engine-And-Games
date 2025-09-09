using Engine.ECS.Components.PositionHandling;
using Engine.Types;
using System.Linq;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Directions;

public class BehaviorSetDirectionToTarget : Behavior
{
    public Direction Direction { get; set; }
    public int PossibleAngles { get; set; }
    public IntVector2 RelativePosition { get; set; }

    public BehaviorSetDirectionToTarget(Direction direction, int possibleAngles = 0, IntVector2 relativePosition = default)
    {
        PossibleAngles = possibleAngles;
        Direction = direction;
        RelativePosition = relativePosition;
    }

    public override void Action()
    {
        var relativePosition = RelativePosition;
        if (Owner.TargetPool.TargetList.FirstOrDefault().Sprite.IsFlipped)
            relativePosition = RelativePosition.MirrorX();

        Direction.SetAngleDirectionTo(Owner.TargetPool.TargetList.FirstOrDefault(), relativePosition);
        if (PossibleAngles > 0)
            Direction.RoundAngle(PossibleAngles);
    }
}
