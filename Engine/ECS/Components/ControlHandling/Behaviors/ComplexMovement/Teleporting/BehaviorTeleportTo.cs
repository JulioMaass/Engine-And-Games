using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Engine.ECS.Components.ControlHandling.Behaviors.ComplexMovement.Teleporting;

public class BehaviorTeleportTo : Behavior
{
    Entity TargetEntity { get; set; }
    IntVector2 RelativePosition { get; set; }
    bool FacingMirrorsX { get; set; }

    public BehaviorTeleportTo(Entity entity, IntVector2 relativePosition, bool facingMirrorsX = false)
    {
        TargetEntity = entity;
        RelativePosition = relativePosition;
        FacingMirrorsX = facingMirrorsX;
    }

    public override void Action()
    {
        var invertX = FacingMirrorsX && TargetEntity.Facing.X == -1;
        var position = new IntVector2(invertX ? -RelativePosition.X : RelativePosition.X, RelativePosition.Y);
        Owner.Position.Pixel = TargetEntity.Position.Pixel + position;
    }
}
