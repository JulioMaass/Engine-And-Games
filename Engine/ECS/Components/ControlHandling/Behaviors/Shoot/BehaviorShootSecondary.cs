namespace Engine.ECS.Components.ControlHandling.Behaviors.Shoot;

public class BehaviorShootSecondary : Behavior
{
    public override void Action()
    {
        Owner.SecondaryShooter.CheckToShoot();
    }
}
