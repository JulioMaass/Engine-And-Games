using Engine.ECS.Components.ControlHandling.States;
using Microsoft.Xna.Framework;

namespace MMDB.GameSpecific.States.Enemy;

public class StateEnemyJumpAndFall : State
{
    public StateEnemyJumpAndFall()
    {
        Name = "JumpAndFall";
    }

    public override bool StartCondition()
    {
        return Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }

    public override bool KeepCondition()
    {
        if (Owner.Speed.Y < 0)
            return true;
        return !Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }

    public override bool PostProcessingKeepCondition()
    {
        return KeepCondition();
    }

    public override void StateSettingBehavior()
    {
        Owner.Speed.Force = new Vector2(Owner.Speed.MoveSpeed * Owner.Facing.X, 0);
        Owner.Speed.SetYSpeed(-Owner.Speed.JumpSpeed);
    }

    public override void Behavior()
    {
        Owner.Speed.SetXSpeed(Owner.Speed.Force.X);
    }
}