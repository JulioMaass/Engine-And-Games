namespace Engine.ECS.Components.ControlHandling.Behaviors.State;

public class BehaviorCancelCommandedState : Behavior
{
    public override void Action()
    {
        Owner.StateManager.CancelCommandedState();
    }
}
