using Engine.ECS.Components.ControlHandling.Behaviors;

namespace Engine.ECS.Components.ShootingHandling.ShootActions;

public class ShootInEightDirections : Behavior // TODO: Move to a behavior folder (ShootActions are not a thing anymore)
{
    public override void Action()
    {
        Owner.Shooter.ShootInAllDirections(8);
    }
}
