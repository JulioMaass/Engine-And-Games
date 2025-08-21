namespace Engine.ECS.Components.ControlHandling.Behaviors.Facing;

public class BehaviorMirrorXFacing : Behavior
{
    public override void Action()
    {
        Owner.Facing.InvertX();
    }
}
