namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorShoot : Behavior
{
    public override void Action()
    {
        Owner.Shooter.CheckToShoot();
    }
}
