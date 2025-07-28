namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorResetFrame : Behavior
{
    public override void Action()
    {
        Owner.StateManager.CurrentState.Frame = 0;
    }
}
