using Engine.ECS.Components.PhysicsHandling;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorSetYSpeed(float ySpeed) : Behavior
{
    public float YSpeed { get; set; } = ySpeed;

    public override void Action()
    {
        Owner.Speed.SetYSpeed(YSpeed);
    }
}
