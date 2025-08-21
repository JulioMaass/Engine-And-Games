namespace Engine.ECS.Components.ControlHandling.Behaviors.State;

public class BehaviorResetStateFrame : Behavior
{
    public override void Action()
    {
        Owner.StateManager.CurrentState.Frame = 0;
    }
}
