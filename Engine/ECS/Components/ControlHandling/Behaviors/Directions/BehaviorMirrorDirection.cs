using Engine.ECS.Components.PositionHandling;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Directions;

public class BehaviorMirrorDirection : Behavior
{
    public Direction Direction { get; set; }

    public BehaviorMirrorDirection(Direction direction)
    {
        Direction = direction;
    }

    public override void Action()
    {
        Direction.Angle = Direction.Angle.GetMirrorX();
    }
}
