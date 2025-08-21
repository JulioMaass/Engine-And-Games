namespace Engine.ECS.Components.ControlHandling.Behaviors.Speed;

public class BehaviorDecelerateSpeed : Behavior
{
    public int Frames { get; set; }

    public BehaviorDecelerateSpeed(int frames)
    {
        Frames = frames;
    }

    public override void Action()
    {
        var percentage = (Frames - Owner.StateManager.CurrentState.Frame) / (float)Frames;
        if (percentage < 0)
            percentage = 0;
        var reducedSpeed = Owner.Speed.Value * percentage;
        Owner.Speed.SetSpeed(reducedSpeed);
    }
}
