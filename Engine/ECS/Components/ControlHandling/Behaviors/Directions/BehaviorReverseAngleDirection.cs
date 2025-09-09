using Engine.ECS.Components.PositionHandling;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Directions;

public class BehaviorReverseAngleDirection : Behavior
{
    public Direction Direction { get; set; }

    public BehaviorReverseAngleDirection(Direction direction)
    {
        Direction = direction;
    }

    public override void Action()
    {
        Direction.Angle = Direction.Angle.Reverse();
    }
}
