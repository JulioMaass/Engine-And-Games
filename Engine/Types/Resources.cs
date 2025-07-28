using System.Collections.Generic;
using System.Linq;

namespace Engine.Types;

public class Resources
{
    public List<Resource> List { get; } = new();

    public void AddNew(ResourceType resourceType, int maxValue, int amount)
        => List.Add(Resource.New(resourceType, maxValue, amount));

    public Resource GetResource(ResourceType resourceType)
        => List.Find(r => r.ResourceType == resourceType);

    public int GetAmount(ResourceType resourceType)
        => GetResource(resourceType).Amount;

    public void SetAmount(ResourceType resourceType, int amount)
        => GetResource(resourceType).Amount = 0;

    public bool HasResource(ResourceType resourceType, int amount)
    {
        if (!List.Exists(r => r.ResourceType == resourceType))
            return false;
        return List.Find(r => r.ResourceType == resourceType).Amount >= amount;
    }

    public void AddAmount(ResourceType resourceType, int amount)
    {
        List.FirstOrDefault(r => r.ResourceType == resourceType).Add(amount);
    }
}
