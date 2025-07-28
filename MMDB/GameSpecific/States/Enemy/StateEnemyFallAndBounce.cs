using Engine.ECS.Components.ControlHandling.States;
using Engine.Helpers;
using Engine.Types;

namespace MMDB.GameSpecific.States.Enemy;

public class StateEnemyFallAndBounce : State
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
        // Turn around if there is a solid in front // TODO: Turn into a reusable function (behavior)
        var dirX = Owner.Facing.X;
        var collisionDirectionX = IntVector2.New(dirX, 0);
        if (Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(collisionDirectionX))
            Owner.Facing.InvertX();
        Owner.Speed.SetXSpeed(Owner.Facing.X * Owner.Speed.MoveSpeed);

        // Set to jump speed if there is a solid below // TODO: Turn into a reusable function (behavior)
        var dirY = Owner.Speed.Value.Y.GetSign();
        var collisionDirectionY = IntVector2.New(0, dirY);
        if (Owner.Speed.Value.Y > 0)
            if (Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(collisionDirectionY))
                Owner.Speed.SetYSpeed(-Owner.Speed.JumpSpeed);
            else
                Owner.Speed.JumpSpeed = Owner.Speed.Value.Y;
    }
}
