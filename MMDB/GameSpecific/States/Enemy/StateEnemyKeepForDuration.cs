using Engine.ECS.Components.ControlHandling.States;

namespace MMDB.GameSpecific.States.Enemy;

public class StateEnemyKeepForDuration : State
{
    // TODO: Turn this into an optional KeepCondition and make a function AddKeepCondition
    private int Duration { get; }

    public StateEnemyKeepForDuration(int duration)
    {
        Duration = duration;
    }

    public override bool StartCondition()
    {
        return true;
    }

    public override bool KeepCondition()
    {
        return Frame < Duration;
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
    }
}
