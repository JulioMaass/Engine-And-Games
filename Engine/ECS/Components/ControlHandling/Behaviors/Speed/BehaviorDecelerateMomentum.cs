
using Microsoft.Xna.Framework;
using System;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Speed;

public class BehaviorDecelerateMomentum : Behavior
{
    public int FramesToStop { get; set; }
    private bool Triggered { get; set; }
    private Vector2 InitialSpeed { get; set; }

    public BehaviorDecelerateMomentum(int framesToStop)
    {
        FramesToStop = framesToStop;
    }

    public override void Action()
    {
        if (!Triggered)
        {
            InitialSpeed = Owner.Speed.Value;
            Triggered = true;
        }

        var deceleration = InitialSpeed / FramesToStop;
        Owner.Speed.SetSpeed(Owner.Speed.Value - deceleration);
        if (Math.Sign(Owner.Speed.Value.X) != Math.Sign(InitialSpeed.X))
            Owner.Speed.SetSpeed(0, 0);
    }
}
