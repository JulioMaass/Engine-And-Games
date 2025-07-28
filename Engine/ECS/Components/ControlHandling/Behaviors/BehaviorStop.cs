using Engine.Helpers;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorStop : Behavior
{
    public Axes AxesToStop { get; set; }

    public BehaviorStop(Axes axesToStop = Axes.Both)
    {
        AxesToStop = axesToStop;
    }

    public override void Action()
    {
        if (AxesToStop.HasFlag(Axes.X))
            Owner.Speed.SetXSpeed(0);
        if (AxesToStop.HasFlag(Axes.Y))
            Owner.Speed.SetYSpeed(0);
    }
}
