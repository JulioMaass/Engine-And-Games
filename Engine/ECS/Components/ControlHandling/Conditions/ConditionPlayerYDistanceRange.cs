using Engine.ECS.Entities;
using System;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionPlayerYDistanceRange : Condition
{
    private int DistanceNear { get; }
    private int DistanceFar { get; }

    public ConditionPlayerYDistanceRange(int distanceNear, int distanceFar)
    {
        DistanceNear = distanceNear;
        DistanceFar = distanceFar;
    }

    protected override bool IsTrue()
    {
        var distanceY = 0;
        if (EntityManager.PlayerEntity != null)
            distanceY = Math.Abs(EntityManager.PlayerEntity.Position.Pixel.Y - Owner.Position.Pixel.Y);

        return distanceY >= DistanceNear && distanceY < DistanceFar;
    }
}