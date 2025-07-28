namespace Engine.ECS.Components.ControlHandling.Behaviors;

public class BehaviorSnapToFloor : Behavior
{
    public override void Action()
    {
        if (Owner.Physics.SolidCollisionChecking.CollidesWithSolidWithPixelSpeed((0, Owner.CollisionBox.Size.Height)))
            Owner.Physics.SolidCollidingMovement.MoveToSolidY(Owner.CollisionBox.Size.Height);
    }
}
