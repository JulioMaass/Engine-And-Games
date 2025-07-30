using Engine.ECS.Entities.EntityCreation;

namespace Engine.ECS.Components.VisualsHandling;

public class BloomSource : Component
{
    public float Intensity { get; private set; }

    public BloomSource(Entity owner, float intensity)
    {
        Owner = owner;
        Intensity = intensity;
    }
}
