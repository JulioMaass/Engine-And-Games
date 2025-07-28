namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorCancelCommandedState : Behavior
{
    public override void Action()
    {
        Owner.StateManager.CancelCommandedState();
    }
}
