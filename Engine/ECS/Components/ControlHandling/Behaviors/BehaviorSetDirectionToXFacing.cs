namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorSetDirectionToXFacing : Behavior
{
    public override void Action()
    {
        Owner.MoveDirection.Angle = Owner.Facing.X == -1 ? 180000 : 0;
    }
}
