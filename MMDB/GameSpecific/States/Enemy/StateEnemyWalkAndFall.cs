using Engine.ECS.Components.ControlHandling.States;
using Engine.Types;

namespace MMDB.GameSpecific.States.Enemy;

public class StateEnemyWalkAndFall : State
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
        var collisionDirection = IntVector2.New(Owner.Facing.X, 0);
        if (Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed(collisionDirection))
            Owner.Facing.InvertX();

        Owner.Speed.SetXSpeed(Owner.Facing.X * Owner.Speed.MoveSpeed);
    }
}
