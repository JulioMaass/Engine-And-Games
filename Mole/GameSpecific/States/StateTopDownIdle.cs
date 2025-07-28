using Engine.ECS.Components.ControlHandling.States;

namespace Mole.GameSpecific.States;

public class StateTopDownIdle : State
{
    public StateTopDownIdle()
    {
        DirectionFrames = 1;
    }

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

    public override void Behavior()
    {
        Owner.Speed.SetXSpeed(0);
        Owner.Speed.SetYSpeed(0);
    }
}
