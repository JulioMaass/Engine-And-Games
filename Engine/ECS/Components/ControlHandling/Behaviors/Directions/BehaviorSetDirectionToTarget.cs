using Engine.ECS.Components.PositionHandling;
using Engine.Types;
using System;
using System.Linq;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Directions;

public class BehaviorSetDirectionToTarget : Behavior
{
    public Direction Direction { get; set; }
    public int PossibleAngles { get; set; }
    public IntVector2 RelativePosition { get; set; }
    public Func<IntVector2> RelativePositionGetter { get; set; }

    public BehaviorSetDirectionToTarget(Direction direction, int possibleAngles = 0)
    {
        PossibleAngles = possibleAngles;
        Direction = direction;
    }

    public BehaviorSetDirectionToTarget(Direction direction, int possibleAngles, IntVector2 relativePosition)
    {
        PossibleAngles = possibleAngles;
        Direction = direction;
        RelativePosition = relativePosition;
    }

    public BehaviorSetDirectionToTarget(Direction direction, int possibleAngles, Func<IntVector2> relativePositionGetter)
    {
        PossibleAngles = possibleAngles;
        Direction = direction;
        RelativePositionGetter = relativePositionGetter;
    }

    public override void Action()
    {
        var relativePosition = RelativePosition;
        if (RelativePositionGetter != null)
            relativePosition = RelativePositionGetter();
        if (Owner.TargetPool.TargetList.FirstOrDefault().Sprite.IsFlipped)
            relativePosition = RelativePosition.MirrorX();

        Direction.SetAngleDirectionTo(Owner.TargetPool.TargetList.FirstOrDefault(), relativePosition);
        if (PossibleAngles > 0)
            Direction.RoundAngle(PossibleAngles);
    }
}
