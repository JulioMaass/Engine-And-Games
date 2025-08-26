using Engine.ECS.Entities.EntityCreation;

namespace Engine.ECS.Components.PhysicsHandling;

public class SolidBehavior : Component
{
    public SolidType SolidType { get; set; }
    public SolidInteractionType SolidInteractionType { get; set; }
    public MomentumType MomentumType { get; set; }

    public SolidBehavior(Entity owner, SolidType solidType, SolidInteractionType solidInteractionType, MomentumType momentumType)
    {
        Owner = owner;
        SolidType = solidType;
        SolidInteractionType = solidInteractionType;
        MomentumType = momentumType;
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

public enum MomentumType
{
    StopHitSpeedOnly, // Only stop the speed that caused the hit (e.g. if moving right and down, and hit a wall on the right, only stop right speed)
    StopBothAxesOnHit // Stop both X and Y speed on any wall hit
}
