using Engine.Helpers;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorSwitchGravity : Behavior
{
    public Switch Switch { get; set; }

    public BehaviorSwitchGravity(Switch @switch)
    {
        Switch = @switch;
    }

    public override void Action()
    {
        Owner.Gravity.IsAffectedByGravity = Switch == Switch.On;
    }
}
