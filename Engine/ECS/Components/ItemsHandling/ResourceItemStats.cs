using Engine.ECS.Entities.EntityCreation;
using Engine.Types;

namespace Engine.ECS.Components.ItemsHandling;

public class ResourceItemStats : Component
{
    public ResourceType ResourceType { get; set; }
    public int Amount { get; set; }

    public ResourceItemStats(Entity owner)
    {
        Owner = owner;
    }
}
