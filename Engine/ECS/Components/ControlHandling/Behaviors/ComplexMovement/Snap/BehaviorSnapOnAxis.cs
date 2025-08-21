using Engine.Helpers;
using Engine.Types;

namespace Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Snap;

public class BehaviorSnapOnAxis : Behavior
{
    public Axis Axis { get; set; }

    public BehaviorSnapOnAxis(Axis axis)
    {
        Axis = axis;
    }

    public override void Action()
    {
        var collisionBoxDir = IntVector2.GetDirFromAxis(Axis) * Owner.CollisionBox.Size;
        if (Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(collisionBoxDir))
            Owner.Physics.SolidCollidingMovement.MoveToSolid(collisionBoxDir);
        else if (Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(-collisionBoxDir))
            Owner.Physics.SolidCollidingMovement.MoveToSolid(-collisionBoxDir);
    }
}
