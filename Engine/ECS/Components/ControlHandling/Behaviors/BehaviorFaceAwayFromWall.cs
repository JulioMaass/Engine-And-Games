using Engine.Types;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorFaceAwayFromWall : Behavior
{
    public override void Action()
    {
        var width = Owner.CollisionBox.Size.Width;
        if (Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(new IntVector2(width, 0)))
            Owner.Facing.InvertX();
    }
}
