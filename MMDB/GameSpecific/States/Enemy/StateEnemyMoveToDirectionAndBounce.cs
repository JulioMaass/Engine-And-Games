using Engine.ECS.Components.ControlHandling.States;
using Engine.Helpers;
using Engine.Types;

namespace MMDB.GameSpecific.States.Enemy;

public class StateEnemyMoveToDirectionAndBounce : State
{
    public override bool StartCondition()
    {
        return true;
    }

    public override bool KeepCondition()
    {
        return true;
    }

    public override bool PostProcessingKeepCondition()
    {
        return true;
    }

    public override void Behavior()
    {
        var dirX = Owner.MoveDirection.GetXLength().GetSign();
        var dirY = Owner.MoveDirection.GetYLength().GetSign();

        var collisionDirectionX = IntVector2.New(dirX, 0);
        if (Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(collisionDirectionX))
            Owner.MoveDirection.MirrorX();
        var collisionDirectionY = IntVector2.New(0, dirY);
        if (Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(collisionDirectionY))
            Owner.MoveDirection.MirrorY();

        // TODO: This can make the enemy move along the wall (ex. if speed is (5, 5) and distance to wall is 3, the enemy will move 2 units along the wall (5, 3))
        // TODO: If enemy moves along the wall, past its edge, it may not bounce
        Owner.Speed.SetMoveSpeedToCurrentDirection();
    }
}
