namespace Engine.ECS.Components.ControlHandling.Behaviors.Speed;

public class BehaviorKnockbackMovement : Behavior
{
    public override void Action()
    {
        Owner.Speed.SetXSpeed(Owner.Speed.KnockbackSpeed * -Owner.Facing.X);
    }
}
