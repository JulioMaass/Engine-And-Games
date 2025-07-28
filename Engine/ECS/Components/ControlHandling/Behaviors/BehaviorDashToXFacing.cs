namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorDashToXFacing : Behavior
{
    public override void Action()
    {
        Owner.Speed.SetXSpeed(Owner.Speed.DashSpeed * Owner.Facing.X);
    }
}
