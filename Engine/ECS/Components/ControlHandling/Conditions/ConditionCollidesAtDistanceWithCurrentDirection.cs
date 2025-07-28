using Engine.Types;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionCollidesAtDistanceWithCurrentDirection : Condition
{
    private int Distance { get; }

    public ConditionCollidesAtDistanceWithCurrentDirection(int distance)
    {
        Distance = distance;
    }

    protected override bool IsTrue()
    {
        var collides = false;
        // Check at full distance
        var xDistance = Owner.MoveDirection.GetXLength() * Distance;
        var yDistance = Owner.MoveDirection.GetYLength() * Distance;
        var speed = new IntVector2((int)xDistance, (int)yDistance);
        if (Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(speed))
            collides = true;

        // Check at midway distances // TODO: Add a function that does this inside SolidCollisionChecking
        for (var i = 0; i < Distance; i += 16)
        {
            var xMidwayDistance = Owner.MoveDirection.GetXLength() * i;
            var yMidwayDistance = Owner.MoveDirection.GetYLength() * i;
            var midwaySpeed = new IntVector2((int)xMidwayDistance, (int)yMidwayDistance);
            if (Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(midwaySpeed))
                collides = true;
        }

        return collides;
    }
}