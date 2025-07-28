using Engine.ECS.Components.ControlHandling.States;

namespace SpaceMiner.GameSpecific.States;

public class StatePlayerControl : State
{
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
        // TODO: Diagonals need to be slower
        if (Owner.PlayerControl.Up)
            Owner.Speed.SetYSpeed(-Owner.Speed.MoveSpeed);
        else if (Owner.PlayerControl.Down)
            Owner.Speed.SetYSpeed(Owner.Speed.MoveSpeed);
        else
            Owner.Speed.SetYSpeed(0);

        if (Owner.PlayerControl.Left)
            Owner.Speed.SetXSpeed(-Owner.Speed.MoveSpeed);
        else if (Owner.PlayerControl.Right)
            Owner.Speed.SetXSpeed(Owner.Speed.MoveSpeed);
        else
            Owner.Speed.SetXSpeed(0);

        // Attack
        AttackFrame--;
        var fireRate = (int)(Owner.Shooter.AutoFireRate / (1 + Owner.StatsManager.GetAddedFloatStats(stats => stats.ExtraAttackSpeed)));
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