namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorChangeMoveSpeed : Behavior
{
    private float MoveSpeed { get; }

    public BehaviorChangeMoveSpeed(float moveSpeed)
    {
        MoveSpeed = moveSpeed;
    }

    public override void Action()
    {
        Owner.Speed.MoveSpeed = MoveSpeed;
    }
}
