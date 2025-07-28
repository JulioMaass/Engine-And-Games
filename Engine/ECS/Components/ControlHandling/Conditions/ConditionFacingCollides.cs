using System;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionFacingCollides : Condition
{
    protected override bool IsTrue() // Returns true if the entity can't move in any direction
    {
        var xDir = Math.Sign(Owner.Facing.X);
        return Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed((xDir, 0));
    }
}
