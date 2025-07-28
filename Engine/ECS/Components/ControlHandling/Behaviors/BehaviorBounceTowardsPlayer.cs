using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Managers.Graphics;
using Engine.Types;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorBounceTowardsPlayer : Behavior
{
    public override void Action() // TODO: If this code stays, unmacaronize it
    {
        var didBounceX = false;
        var didBounceY = false;

        // Check X bounce
        var dirX = Owner.MoveDirection.Angle.GetXDir();
        var collisionDirectionX = IntVector2.New(dirX, 0);
        if (Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(collisionDirectionX)
            || !Camera.GetSpawnScreenLimits().Contains(Owner.Position.Pixel + collisionDirectionX * 3))
            didBounceX = true;

        // Check Y bounce
        var dirY = Owner.MoveDirection.Angle.GetYDir();
        var collisionDirectionY = IntVector2.New(0, dirY);
        if (Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(collisionDirectionY)
            || !Camera.GetSpawnScreenLimits().Contains(Owner.Position.Pixel + collisionDirectionY * 3))
            didBounceY = true;

        if (didBounceX || didBounceY)
        {
            var originalAngle = Owner.MoveDirection.Angle.Value;
            if (EntityManager.PlayerEntity != null)
                Owner.MoveDirection.SetAngleDirectionTo(EntityManager.PlayerEntity);
            var newCollisionDirection = IntVector2.New(Owner.MoveDirection.GetXLength().GetSign(), Owner.MoveDirection.GetYLength().GetSign());
            if (!Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(newCollisionDirection))
                Owner.MoveDirection.SetAngleDirectionTo(EntityManager.PlayerEntity);
            else
            {
                Owner.MoveDirection.Angle = originalAngle;
                if (didBounceX)
                    Owner.MoveDirection.MirrorX();
                if (didBounceY)
                    Owner.MoveDirection.MirrorY();
            }
        }
        Owner.Speed.SetMoveSpeedToCurrentDirection();
    }
}
