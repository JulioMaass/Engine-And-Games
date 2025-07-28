using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Entities;
using Engine.Types;

namespace MMDB.GameSpecific.States.Enemy;

public class StateEnemyTurnAngleAndMove : State
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
        var intendedDirection = Owner.MoveDirection.Angle;
        if (EntityManager.PlayerEntity != null)
            intendedDirection = Angle.GetDirection(Owner, EntityManager.PlayerEntity);
        Owner.MoveDirection.TurnTowardsAngle(intendedDirection.Value);

        Owner.Speed.SetMoveSpeedToCurrentDirection(Owner.Speed.MoveSpeed);
    }
}
