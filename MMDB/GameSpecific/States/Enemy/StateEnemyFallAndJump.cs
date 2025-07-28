using Engine.ECS.Components.ControlHandling.States;
using Engine.Helpers;
using Engine.Types;

namespace MMDB.GameSpecific.States.Enemy;

public class StateEnemyFallAndJump : State
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
        // TODO: Same as StateEnemyFallAndBounce, but has fixed jump speed instead of updating it (merge both?)
        // Turn around if there is a solid in front
        var dirX = Owner.Facing.X;
        var collisionDirectionX = IntVector2.New(dirX, 0);
        if (Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(collisionDirectionX))
            Owner.Facing.InvertX();
        Owner.Speed.SetXSpeed(Owner.Facing.X * Owner.Speed.MoveSpeed);


        // Set to jump speed if there is a solid below
        var dirY = Owner.Speed.Value.Y.GetSign();
        var collisionDirectionY = IntVector2.New(0, dirY);
        if (Owner.Speed.Value.Y > 0)
            if (Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(collisionDirectionY))
                Owner.Speed.SetYSpeed(-Owner.Speed.JumpSpeed);
    }
}
