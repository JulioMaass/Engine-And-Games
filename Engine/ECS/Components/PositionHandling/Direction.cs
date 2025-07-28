using Engine.ECS.Entities.EntityCreation;

namespace Engine.ECS.Components.PositionHandling;

public class Direction : AngleComponent
{
    public Direction(Entity owner, int angle)
    {
        Owner = owner;
        Angle = new Types.Angle(angle);
    }
}
