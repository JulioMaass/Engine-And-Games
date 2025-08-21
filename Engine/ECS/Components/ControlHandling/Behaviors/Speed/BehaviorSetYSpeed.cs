namespace Engine.ECS.Components.ControlHandling.Behaviors.Speed;

public class BehaviorSetYSpeed(float ySpeed) : Behavior
{
    public float YSpeed { get; set; } = ySpeed;

    public override void Action()
    {
        Owner.Speed.SetYSpeed(YSpeed);
    }
}
