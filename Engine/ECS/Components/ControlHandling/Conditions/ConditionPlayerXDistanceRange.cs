using Engine.ECS.Entities;
using System;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionPlayerXDistanceRange : Condition
{
    private int DistanceNear { get; }
    private int DistanceFar { get; }

    public ConditionPlayerXDistanceRange(int distanceNear, int distanceFar)
    {
        DistanceNear = distanceNear;
        DistanceFar = distanceFar;
    }

    protected override bool IsTrue()
    {
        var distanceX = 0;
        if (EntityManager.PlayerEntity != null)
            distanceX = Math.Abs(EntityManager.PlayerEntity.Position.Pixel.X - Owner.Position.Pixel.X);

        return distanceX >= DistanceNear && distanceX < DistanceFar;
    }
}