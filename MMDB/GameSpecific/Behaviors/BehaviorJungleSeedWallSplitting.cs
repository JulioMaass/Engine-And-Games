using Engine.ECS.Components.ControlHandling.Behaviors;
using Engine.ECS.Entities;
using MMDB.GameSpecific.Entities.Weapons;
using System;
using System.Diagnostics;

namespace MMDB.GameSpecific.Behaviors;

public class BehaviorJungleSeedWallSplitting : Behavior
{
    public override void Action()
    {
        if (Owner.Speed.X == 0 && Owner.Speed.Y == 0)
        {
            EntityManager.DeleteEntity(Owner);
            if (((JungleSeed)Owner).SplitLevel <= 0)
                return;

            var originalAngle = Owner.MoveDirection.Angle.Value;
            var seed1 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
            var seed2 = (JungleSeed)EntityManager.CreateEntityAt(typeof(JungleSeed), Owner.Position.Pixel);
            seed1.SetLevel(((JungleSeed)Owner).SplitLevel - 1);
            seed2.SetLevel(((JungleSeed)Owner).SplitLevel - 1);

            if (originalAngle % 90000 == 0)
            {
                // Split from orthogonal direction
                seed1.AddMoveDirection(originalAngle + 90000 + 45000);
                seed2.AddMoveDirection(originalAngle + 180000 + 45000);
            }
            else
            {
                // Split from diagonal direction
                var horizontalDirection = Math.Sign(Owner.MoveDirection.Angle.GetXLength());
                var verticalDirection = Math.Sign(Owner.MoveDirection.Angle.GetYLength());
                var horizontalHits = Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed((horizontalDirection, 0));
                var verticalHits = Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed((0, verticalDirection));

                // Seed1 goes to direction perpendicular to wall
                if (horizontalHits)
                    seed1.AddMoveDirection(90000 + horizontalDirection * 90000);
                else if (verticalHits)
                    seed1.AddMoveDirection(0 - verticalDirection * 90000);
                else
                    Debugger.Break(); // No wall detected. Shouldn't happen.

                // Seed2 goes to diagonal direction mirrored from wall
                seed2.AddMoveDirection(originalAngle);
                if (horizontalHits)
                    seed2.MoveDirection.MirrorX();
                else if (verticalHits)
                    seed2.MoveDirection.MirrorY();
                else
                    Debugger.Break(); // No wall detected. Shouldn't happen.
            }

            seed1.Speed.SetMoveSpeedToCurrentDirection();
            seed2.Speed.SetMoveSpeedToCurrentDirection();
            seed1.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
            seed2.Alignment.OwningEntity = Owner.Alignment.OwningEntity;
        }
    }
}
