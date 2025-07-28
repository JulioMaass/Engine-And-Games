using Engine.Types;
using System;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionXDistanceFromSpawnBiggerThan : Condition
{
    private RandomInt Distance { get; }

    public ConditionXDistanceFromSpawnBiggerThan(RandomInt distance)
    {
        Distance = distance;
    }

    protected override bool IsTrue()
    {
        var currentDistance = Math.Abs(Owner.Position.Pixel.X - Owner.Position.StartingPosition.X);
        return currentDistance >= Distance;
    }
}
