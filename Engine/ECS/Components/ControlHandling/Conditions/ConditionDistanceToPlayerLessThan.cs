using Engine.ECS.Entities;
using Engine.Types;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionDistanceToPlayerLessThan : Condition
{
    private int Distance { get; }

    public ConditionDistanceToPlayerLessThan(int distance)
    {
        Distance = distance;
    }

    protected override bool IsTrue()
    {
        if (EntityManager.PlayerEntity == null)
            return false;
        var currentDistance = IntVector2.GetDistance(
            Owner.Position.Pixel,
            EntityManager.PlayerEntity.Position.Pixel
        );
        return currentDistance < Distance;
    }
}