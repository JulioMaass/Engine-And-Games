using System;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionYSpeedBiggerThan : Condition
{
    private float Speed { get; }

    public ConditionYSpeedBiggerThan(float speed)
    {
        Speed = speed;
    }

    protected override bool IsTrue()
    {
        return Math.Abs(Owner.Speed.Y) > Speed;
    }
}