using Engine.ECS.Entities;
using System;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorJumpToDestination : Behavior
{
    private float JumpYForce { get; }

    public BehaviorJumpToDestination(float jumpYForce)
    {
        JumpYForce = jumpYForce;
    }

    public override void Action()
    {
        //Reach with ySpeed // TODO: Copied from ShootAtPlayerWithGravityAtTime, make it a function
        var distance = EntityManager.PlayerEntity.Position.Pixel - Owner.Position.Pixel;
        var ySpeed = -JumpYForce;
        var timeToPeak = -ySpeed / Owner.Gravity.Force;
        var peakHeight = -ySpeed * timeToPeak - 0.5f * Owner.Gravity.Force * timeToPeak * timeToPeak;
        var totalHeightDifference = peakHeight + distance.Y;
        var timeToFall = (float)Math.Sqrt(2 * totalHeightDifference / Owner.Gravity.Force);
        if (float.IsNaN(timeToFall)) // If target is unreachable, top of the arc will align with target
            timeToFall = 0;
        var totalTime = timeToPeak + timeToFall;
        var xSpeed = distance.X / totalTime;
        Owner.Speed.SetXSpeed(xSpeed);
        Owner.Speed.SetYSpeed(ySpeed);
    }
}
