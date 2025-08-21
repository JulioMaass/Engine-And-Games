namespace Engine.ECS.Components.ControlHandling.Behaviors.Speed;

public class BehaviorDashToXFacing : Behavior
{
    public override void Action()
    {
        Owner.Speed.SetXSpeed(Owner.Speed.DashSpeed * Owner.Facing.X);
    }
}
