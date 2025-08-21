namespace Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Facing;

public class BehaviorMoveAwayFromXFacing : Behavior // TODO: Use this for hurt states
{
    private float Speed { get; }

    public BehaviorMoveAwayFromXFacing(float speed)
    {
        Speed = speed;
    }

    public override void Action()
    {
        Owner.Speed.SetXSpeed(Speed * -Owner.Facing.X);
    }
}
