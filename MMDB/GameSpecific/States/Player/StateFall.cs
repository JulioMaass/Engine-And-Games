using Engine.ECS.Components.ControlHandling.States;

namespace MMDB.GameSpecific.States.Player;

public class StateFall : State
{
    public override bool AllowSecondaryState { get; protected set; } = true;

    public override bool StartCondition()
    {
        return !Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }

    public override bool KeepCondition()
    {
        return !Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }

    public override bool PostProcessingKeepCondition()
    {
        return !Owner.Physics.SolidCollisionChecking.IsOnTopOfSolid();
    }

    public override void StateSettingBehavior()
    {
        Owner.Speed.SetYSpeed(0);
    }

    public override void Behavior()
    {
        Owner.Speed.SetXSpeed(Owner.PlayerControl.DirectionX * Owner.Speed.MoveSpeed);
    }
}
