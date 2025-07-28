namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorMirrorXFacing : Behavior
{
    public override void Action()
    {
        Owner.Facing.InvertX();
    }
}
