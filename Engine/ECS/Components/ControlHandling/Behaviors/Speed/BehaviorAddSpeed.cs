namespace Engine.ECS.Components.ControlHandling.Behaviors.Speed;

public class BehaviorAddSpeed : Behavior
{
    private float AddX { get; }
    private float AddY { get; }

    public BehaviorAddSpeed(float addX, float addY)
    {
        AddX = addX;
        AddY = addY;
    }

    public override void Action()
    {
        Owner.Speed.AddXSpeed(AddX);
        Owner.Speed.AddYSpeed(AddY);
    }
}
