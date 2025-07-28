using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Engine.ECS.Components.PositionHandling;

public class RelativePosition : Component
{
    public IntVector2 ShotCreation { get; set; }

    public RelativePosition(Entity owner)
    {
        Owner = owner;
    }
}