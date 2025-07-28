using Engine.ECS.Components.ControlHandling.States;

namespace Mole.GameSpecific.States;

public class StateTopDownWalk : State
{
    public StateTopDownWalk()
    {
        DirectionFrames = 10;
    }

    public override bool StartCondition()
    {
        var commandWalk = Owner.PlayerControl.DirectionX != 0 || Owner.PlayerControl.DirectionY != 0;
        return commandWalk;
    }

    public override bool KeepCondition()
    {
        var commandWalk = Owner.PlayerControl.DirectionX != 0 || Owner.PlayerControl.DirectionY != 0;
        return commandWalk;
    }

    public override bool PostProcessingKeepCondition()
    {
        return true;
    }

    public override void Behavior()
    {
        const float diagonalMultiplier = 0.70710678118f;
        var xSpeed = Owner.PlayerControl.DirectionX * Owner.Speed.MoveSpeed;
        var ySpeed = Owner.PlayerControl.DirectionY * Owner.Speed.MoveSpeed;

        if (Owner.PlayerControl.DirectionX != 0 && Owner.PlayerControl.DirectionY != 0)
        {
            xSpeed *= diagonalMultiplier;
            ySpeed *= diagonalMultiplier;
        }

        Owner.Speed.SetXSpeed(xSpeed);
        Owner.Speed.SetYSpeed(ySpeed);
    }
}
