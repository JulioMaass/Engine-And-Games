using Microsoft.Xna.Framework;

namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorJumpToFacing : Behavior
{
    private Vector2 JumpForce { get; }

    public BehaviorJumpToFacing(Vector2 jumpForce)
    {
        JumpForce = jumpForce;
    }

    public override void Action()
    {
        var facing = Owner.Facing.X;
        var speed = new Vector2(facing * JumpForce.X, JumpForce.Y);
        Owner.Speed.SetSpeed(speed);
    }
}
