using Engine.ECS.Entities.EntityCreation;

namespace Engine.ECS.Components.VisualsHandling;

public class BloomSource : Component
{
    public BloomType BloomType { get; set; }
    public float Intensity { get; private set; }

    public BloomSource(Entity owner, float intensity)
    {
        Owner = owner;
        Intensity = intensity;
        BloomType = BloomType.Bloom;
    }

    public BloomSource(Entity owner, BloomType bloomType)
    {
        Owner = owner;
        Intensity = 0f;
        BloomType = bloomType;
    }
}

public enum BloomType
{
    Shadow,
    Bypass,
    Bloom
}