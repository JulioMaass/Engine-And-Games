using Engine.Helpers;
using System.Diagnostics;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Speed;

public class BehaviorAccelerateToDirection : Behavior
{
    private Axes AccelerateAxes { get; set; }
    private float MaxSpeed { get; set; }

    public BehaviorAccelerateToDirection(Axes accelerateAxes = Axes.Both, float maxSpeed = float.MaxValue)
    {
        AccelerateAxes = accelerateAxes;
        MaxSpeed = maxSpeed;
    }

    public override void Action()
    {
        var accelerateX = false;
        var accelerateY = false;
        if (AccelerateAxes.HasFlag(Axes.X))
            accelerateX = true;
        if (AccelerateAxes.HasFlag(Axes.Y))
            accelerateY = true;
        if (Owner.Speed.Acceleration == 0)
            Debugger.Break(); // No acceleration was added
        Owner.MoveDirection.AddSpeedTowardsAngle(Owner.Speed.Acceleration, accelerateX, accelerateY);
        Owner.MoveDirection.ClampSpeedToAngle(MaxSpeed);
    }
}
