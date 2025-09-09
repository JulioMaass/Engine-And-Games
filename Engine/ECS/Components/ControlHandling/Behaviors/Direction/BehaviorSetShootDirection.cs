namespace Engine.ECS.Components.ControlHandling.Behaviors.Direction;

public class BehaviorSetShootDirection : Behavior
{
    public int Angle { get; set; }

    public BehaviorSetShootDirection(int angle)
    {
        Angle = angle;
    }

    public override void Action()
    {
        Owner.ShootDirection.Angle = Angle;
    }
}
