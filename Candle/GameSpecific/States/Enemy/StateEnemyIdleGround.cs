using Engine.ECS.Components.ControlHandling.States;

namespace Candle.GameSpecific.States.Enemy;

public class StateEnemyIdleGround : State
{
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
    }
}
