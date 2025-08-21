namespace Engine.ECS.Components.ControlHandling.Behaviors.Shoot;

public class BehaviorShoot : Behavior
{
    public override void Action()
    {
        Owner.Shooter.CheckToShoot();
    }
}
