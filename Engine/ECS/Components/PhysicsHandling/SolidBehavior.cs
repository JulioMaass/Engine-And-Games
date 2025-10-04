using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;

namespace Engine.ECS.Components.PhysicsHandling;

public class SolidBehavior : Component
{
    public SolidType SolidType { get; private set; }
    public SolidInteractionType SolidInteractionType { get; set; }
    public MomentumType MomentumType { get; private set; }

    public SolidBehavior(Entity owner, SolidType solidType, SolidInteractionType solidInteractionType, MomentumType momentumType)
    {
        Owner = owner;
        SetSolidType(solidType);
        SolidInteractionType = solidInteractionType;
        MomentumType = momentumType;
    }

    public void SetSolidType(SolidType solidType)
    {
        SolidType = solidType;
        if (solidType == SolidType.Solid)
        {
            EntityListManager.SolidEntities.Add(Owner);
            EntityListManager.SolidTopEntities.Remove(Owner);
        }
        else if (solidType == SolidType.SolidTop)
        {
            EntityListManager.SolidTopEntities.Add(Owner);
            EntityListManager.SolidEntities.Remove(Owner);
        }
    }

    public void SetSolidInteractionType(SolidInteractionType solidInteractionType)
    {
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

public enum MomentumType
{
    StopHitSpeedOnly, // Only stop the speed that caused the hit (e.g. if moving right and down, and hit a wall on the right, only stop right speed)
    StopBothAxesOnHit // Stop both X and Y speed on any wall hit
}
