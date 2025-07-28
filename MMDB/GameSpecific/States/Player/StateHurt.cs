using Engine.ECS.Components.ControlHandling.States;

namespace MMDB.GameSpecific.States.Player;

public class StateHurt : State
{
    private int Duration { get; }
    private float XSpeed { get; }
    private float YSpeed { get; }
    public override bool CanUpdateFacing { get; set; } = false;

    public StateHurt(float xSpeed, int duration)
    {
        XSpeed = xSpeed;
        YSpeed = 0;
        Duration = duration;
    }

    public override bool StartCondition()
    {
        return Owner.PlayerControl.GotHurt.Check() && !Owner.CollisionBox.DefaultHitboxCollidesWithSolid();
    }

    public override bool KeepCondition()
    {
        var nextFrameIsHurt = Frame < Duration;
        return nextFrameIsHurt;
    }

    public override bool PostProcessingKeepCondition()
    {
        return true;
    }

    public override void StateSettingBehavior()
    {
        Owner.Speed.SetYSpeed(YSpeed);
    }

    public override void Behavior()
    {
        Owner.Speed.SetXSpeed(Owner.Facing.X * -XSpeed);
    }
}