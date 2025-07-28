using Engine.ECS.Entities.EntityCreation;

namespace Engine.ECS.Components.PhysicsHandling;

public class SolidBehavior : Component
{
    public SolidType SolidType { get; set; }
    public SolidInteractionType SolidInteractionType { get; set; }

    public SolidBehavior(Entity owner, SolidType solidType, SolidInteractionType solidInteractionType)
    {
        Owner = owner;
        SolidType = solidType;
        SolidInteractionType = solidInteractionType;
    }
}

public enum SolidType
{
    NotSolid,
    Solid,
    SolidTop
}
public enum SolidInteractionType
{
    GoThroughSolids,
    StopOnSolids // Pushable
}