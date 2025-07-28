using Engine.ECS.Components.ControlHandling.SecondaryStates;
using Engine.ECS.Components.ControlHandling.States;

namespace MMDB.GameSpecific.States.Player;

public class StateIdle : State
{
    public override bool AllowSecondaryState { get; protected set; } = true;

    public override bool StartCondition()
    {
        return Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }

    public override bool KeepCondition()
    {
        return Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }

    public override bool PostProcessingKeepCondition()
    {
        return Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }

    public override void Behavior()
    {
        Owner.Speed.SetXSpeed(0);
        Owner.Speed.SetYSpeed(0);

        if (Owner.StateManager.CurrentSecondaryState is not SecondaryStateNone)
            Owner.StateManager.CurrentState.Frame = 0;
    }
}
