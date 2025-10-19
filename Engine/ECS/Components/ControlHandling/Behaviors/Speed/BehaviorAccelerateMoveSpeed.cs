using System;

namespace Engine.ECS.Components.ControlHandling.Behaviors.Speed;

public class BehaviorAccelerateMoveSpeed : Behavior
{
    public BehaviorAccelerateMoveSpeed()
    {
    }

    public override void Action()
    {
        Owner.Speed.AccelerateMoveSpeed();
    }
}
