namespace Engine.ECS.Components.ControlHandling.Behaviors.Speed;

public class BehaviorAccelerateForFrames : Behavior
{
    public int Frames { get; set; }

    public BehaviorAccelerateForFrames(int frames)
    {
        Frames = frames;
    }

    public override void Action()
    {
        var percentage = Owner.StateManager.CurrentState.Frame / (float)Frames;
        if (percentage > 1)
            percentage = 1;
        var reducedSpeed = Owner.Speed.Value * percentage;
        Owner.Speed.SetSpeed(reducedSpeed);
    }
}
