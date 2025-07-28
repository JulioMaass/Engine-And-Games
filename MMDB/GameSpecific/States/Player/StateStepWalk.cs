using Engine.ECS.Components.ControlHandling.States;

namespace MMDB.GameSpecific.States.Player;

public class StateStepWalk : State
{
    public override bool AllowSecondaryState { get; protected set; } = true;

    public StateStepWalk()
    {
        UpdatesFrameOnTransitions = true;
    }

    public override bool StartCondition()
    {
        var commandWalk = Owner.PlayerControl.DirectionX != 0;
        return commandWalk && Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }

    public override bool KeepCondition()
    {
        var basicCondition = Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
        var commandWalk = Owner.PlayerControl.DirectionX != 0;
        var nextFrameIsStep = Owner.StateManager.CurrentState.Frame + 1 < Owner.StateManager.CurrentState.StartUpDuration;
        return basicCondition && (commandWalk || nextFrameIsStep);
    }

    public override bool PostProcessingKeepCondition()
    {
        return Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }

    public override void StateSettingBehavior()
    {
        if (Owner.StateManager.PreviousState is not StateIdle)
            Frame = StartUpDuration;
    }

    public override void StartUpBehavior()
    {
        if (Frame == 0)
        {
            Owner.Speed.SetXSpeed(Owner.PlayerControl.DirectionX * 1);
        }
        else
        {
            Owner.Speed.SetXSpeed(0);
        }
    }

    public override void Behavior()
    {
        Owner.Speed.SetXSpeed(Owner.PlayerControl.DirectionX * Owner.Speed.MoveSpeed);
    }
}
