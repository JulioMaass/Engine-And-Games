using Engine.ECS.Components.ControlHandling.States;

namespace MMDB.GameSpecific.States.Player;

public class StateJump : State
{
    public override bool AllowSecondaryState { get; protected set; } = true;

    public override bool StartCondition()
    {
        return Owner.PlayerControl.Button2Press && Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid() && !Owner.CollisionBox.DefaultHitboxCollidesWithSolid();
    }
    public override bool KeepCondition()
    {
        return Owner.PlayerControl.Button2Hold && Owner.Speed.Y < 0;
    }

    public override bool PostProcessingKeepCondition()
    {
        return KeepCondition();
    }

    public override void StateSettingBehavior()
    {
        Owner.Speed.SetYSpeed(-Owner.Speed.JumpSpeed);
    }
    public override void Behavior()
    {
        Owner.Speed.SetXSpeed(Owner.PlayerControl.DirectionX * Owner.Speed.MoveSpeed);
    }
}