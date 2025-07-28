using Engine.ECS.Components.ControlHandling.States;
using Engine.Types;

namespace MMDB.GameSpecific.States.Player;

public class StateTeleport : State
{
    private int Duration { get; }
    public override bool CanUpdateFacing { get; set; } = false;
    public override bool IsInvincible { get; set; } = true;

    public StateTeleport(int duration)
    {
        Duration = duration;
    }

    public override bool StartCondition()
    {
        return Owner.PlayerControl.StartTeleporting.Check();
    }

    public override bool KeepCondition()
    {
        var nextFrameIsTeleport = Frame < Duration;
        return nextFrameIsTeleport;
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
        Owner.Speed.SetXSpeed(0);
        Owner.Speed.SetYSpeed(0);
    }

    public override IntVector2 GetAnimationOffset()
    {
        var yOffset = (Frame - Duration) * 6;
        return IntVector2.New(0, yOffset);
    }
}