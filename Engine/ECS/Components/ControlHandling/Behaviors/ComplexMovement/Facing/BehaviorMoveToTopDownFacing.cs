namespace Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Facing;

public class BehaviorMoveToTopDownFacing : Behavior
{
    public override void Action()
    {
        var xSpeed = Owner.Facing.X * Owner.Speed.MoveSpeed;
        var ySpeed = Owner.Facing.Y * Owner.Speed.MoveSpeed;
        Owner.Speed.SetSpeed(xSpeed, ySpeed);
    }
}
