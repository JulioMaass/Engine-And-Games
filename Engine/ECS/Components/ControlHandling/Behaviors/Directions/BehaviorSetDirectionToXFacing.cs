using Engine.ECS.Components.PositionHandling;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Directions;

public class BehaviorSetDirectionToXFacing : Behavior
{
    public Direction Direction { get; set; }

    public BehaviorSetDirectionToXFacing(Direction direction)
    {
        Direction = direction;
    }

    public override void Action()
    {
        Direction.Angle = Owner.Facing.X == -1 ? 180000 : 0;
    }
}
