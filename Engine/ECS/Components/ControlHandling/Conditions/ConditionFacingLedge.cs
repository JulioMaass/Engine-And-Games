using Engine.Types;
using System;

namespace Engine.ECS.Components.ControlHandling.Conditions;

public class ConditionFacingLedge : Condition
{
    protected override bool IsTrue()
    {
        var collisionDirection = IntVector2.New(Owner.Facing.X, 0);
        var collisionDirectionLedge = IntVector2.New(Owner.Facing.X, 1);
        var edgePosition = Owner.CollisionBox.GetEdgePosition(collisionDirectionLedge) + IntVector2.PixelDown + collisionDirection;
        if (Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid())
            if (!Owner.Physics.SolidCollisionChecking.IsThereSolidAtPoint(edgePosition))
                return true;
        return false;
    }
}
