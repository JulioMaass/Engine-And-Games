using Engine.ECS.Components.ControlHandling.States;
using Microsoft.Xna.Framework;

namespace Candle.GameSpecific.States.Enemy;

public class StateEnemyJump : State
{
    public float JumpSpeed { get; set; }

    public StateEnemyJump(float jumpSpeed = 0f)
    {
        JumpSpeed = jumpSpeed;
    }

    public override bool StartCondition()
    {
        return Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }

    public override bool KeepCondition()
    {
        return Owner.Speed.Y < 0;
    }

    public override bool PostProcessingKeepCondition()
    {
        return Owner.Speed.Y < 0;
    }

    public override void StateSettingBehavior()
    {
        Owner.Speed.Force = new Vector2(Owner.Speed.MoveSpeed * Owner.Facing.X, 0);

        if (JumpSpeed != 0)
            Owner.Speed.SetYSpeed(-JumpSpeed);
        else
            Owner.Speed.SetYSpeed(-Owner.Speed.JumpSpeed);
    }

    public override void Behavior()
    {
        Owner.Speed.SetXSpeed(Owner.Speed.Force.X);
    }
}