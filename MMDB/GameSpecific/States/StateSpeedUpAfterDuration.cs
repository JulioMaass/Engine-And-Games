using Engine.ECS.Components.ControlHandling.States;

namespace MMDB.GameSpecific.States;

public class StateSpeedUpAfterDuration : State
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

    public override void StateSettingBehavior()
    {
    }

    public override void Behavior()
    {
        if (Owner.StateManager.CurrentState.Frame == 6)
            Owner.Speed.SetSpeed(8.0f * Owner.Facing.X, 0.0f);
    }
}