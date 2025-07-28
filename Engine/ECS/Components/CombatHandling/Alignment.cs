using Engine.ECS.Entities.EntityCreation;

namespace Engine.ECS.Components.CombatHandling;

public class Alignment : Component
{
    public AlignmentType Type { get; }

    public Alignment(Entity owner, AlignmentType type)
    {
        Owner = owner;
        Type = type;
    }
}

public enum AlignmentType
{
    None,
    Friendly,
    Neutral,
    Hostile
}