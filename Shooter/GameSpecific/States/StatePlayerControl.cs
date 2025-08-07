using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.ItemsHandling;

namespace ShooterGame.GameSpecific.States;

public class StatePlayerControl : State
{
    private int DashFrame { get; set; }
    private int AttackFrame { get; set; }
    private bool BufferedAttack { get; set; }

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

    public override void StateSettingBehavior()
    {
    }

    public override void Behavior()
    {
        // TODO: Make an actual state machine for the player
        // Direction // TODO: Make acceleration and deceleration (test 0.2acc and 0.3dec)
        if (Owner.PlayerControl.Left)
            Owner.Speed.SetXSpeed(-Owner.Speed.MoveSpeed);
        else if (Owner.PlayerControl.Right)
            Owner.Speed.SetXSpeed(Owner.Speed.MoveSpeed);
        else
            Owner.Speed.SetXSpeed(0);

        // Jump // TODO: Limit to 1 double jump
        if (Owner.PlayerControl.Button2Press)
        {
            //if (Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid() // On ground
            //    || Owner.StatsManager.CheckForUnlock(stats => stats.DoubleJump)) // Double Jump
            Owner.Speed.SetYSpeed(-Owner.Speed.JumpSpeed);
        }
        else if (!Owner.PlayerControl.Button2Hold && Owner.Speed.Value.Y < 0)
            Owner.Speed.SetYSpeed(0);

        // Dash // TODO: Limit to 1 per jump
        if (Owner.PlayerControl.Button3Press) //&& Owner.StatsManager.CheckForUnlock(stats => stats.Dash))
            DashFrame = 15;
        if (DashFrame > 0)
        {
            DashFrame--;
            Owner.Speed.SetXSpeed(Owner.Speed.DashSpeed * Owner.Facing.X);
            Owner.Speed.SetYSpeed(0);
        }

        // Attack
        AttackFrame--;
        var fireRate = (int)(20 / (1 + StatsManager.GetAddedFloatStats(Owner, stats => stats.ExtraAttackSpeed)));
        if (Owner.PlayerControl.Button1Hold && AttackFrame <= 10)
            BufferedAttack = true;
        if (AttackFrame <= 0 && BufferedAttack)
        {
            AttackFrame = fireRate;
            Owner.Shooter.CheckToShoot();
            BufferedAttack = false;
        }
    }
}