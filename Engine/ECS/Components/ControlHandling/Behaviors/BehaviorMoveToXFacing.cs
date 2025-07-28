namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorMoveToXFacing : Behavior
{
    public override void Action()
    {
        Owner.Speed.SetXSpeed(Owner.Speed.MoveSpeed * Owner.Facing.X);
    }
}
