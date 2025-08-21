namespace Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Direction;

public class BehaviorMoveToCurrentDirection : Behavior
{
    public override void Action()
    {
        Owner.Speed.SetMoveSpeedToCurrentDirection(Owner.Speed.MoveSpeed);
    }
}
