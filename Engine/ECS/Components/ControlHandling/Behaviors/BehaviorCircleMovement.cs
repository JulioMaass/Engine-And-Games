using Engine.Helpers;
using Engine.Types;
using System;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorCircleMovement : Behavior
{
    private Axes AxesToMove { get; set; }
    private int Radius { get; set; }
    private int LoopDuration { get; set; }
    private float CurrentAngle { get; set; }
    private IntVector2 Center { get; set; }

    public BehaviorCircleMovement(Axes axesToMove, int radius, int loopDuration, int startingAngle)
    {
        AxesToMove = axesToMove;
        Radius = radius;
        LoopDuration = loopDuration;
        CurrentAngle = (float)(startingAngle * Math.PI / 180000.0); // Convert degrees to radians
    }

    public override void Action()
    {
        var angularVelocity = (float)(2 * Math.PI) / LoopDuration;
        CurrentAngle += angularVelocity;

        var velocityX = -(float)(angularVelocity * Radius * Math.Sin(CurrentAngle));
        var velocityY = (float)(angularVelocity * Radius * Math.Cos(CurrentAngle));

        if (AxesToMove.HasFlag(Axes.X))
            Owner.Speed.SetXSpeed(velocityX);
        if (AxesToMove.HasFlag(Axes.Y))
            Owner.Speed.SetYSpeed(velocityY);
    }
}
