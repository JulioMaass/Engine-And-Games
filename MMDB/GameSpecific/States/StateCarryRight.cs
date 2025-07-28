using Engine.ECS.Components.ControlHandling.States;

namespace MMDB.GameSpecific.States;

public class StateCarryRight : State
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

    public override void Behavior()
    {
        Owner.Physics.PushAndCarryMovement.UpdateCarriablesOnTop();
        Owner.Physics.PushAndCarryMovement.ConveyorBeltCarry(1);
    }
}