namespace Engine.ECS.Components.ControlHandling.Behaviors.Direction;

public class BehaviorReverseAngleDirection : Behavior
{
    public override void Action()
    {
        Owner.MoveDirection.Angle = Owner.MoveDirection.Angle.Reverse();
    }
}
