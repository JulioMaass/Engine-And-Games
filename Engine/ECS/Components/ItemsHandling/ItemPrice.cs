using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.GlobalManagement;
using Engine.Types;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS.Components.ItemsHandling;

public class ItemPrice : Component
{
    public List<Price> PriceList { get; } = []; // index 0 is price to buy, later indexes are upgrades

    public class Price() // Price may hold multiple resource costs: 10 blue ores + 10 yellow ores
    {
        public Price(ResourceCost resourceCost) : this() =>
            ResourceCosts.Add(resourceCost);

        public List<ResourceCost> ResourceCosts { get; } = [];
    }

    public struct ResourceCost(ResourceType resourceType, int amount)
    {
        public readonly ResourceType ResourceType = resourceType;
        public readonly int Amount = amount;
    }

    public ItemPrice(Entity owner)
    {
        Owner = owner;
    }

    public void AddPrice(params (ResourceType, int)[] resourceCosts)
    {
        var price = new Price();
        foreach (var (resourceType, amount) in resourceCosts)
            price.ResourceCosts.Add(new ResourceCost(resourceType, amount));
        PriceList.Add(price);
    }

    public void AddPrices(ResourceType resourceType, params int[] resourceCosts)
    {
        foreach (var amount in resourceCosts)
            PriceList.Add(new Price(new ResourceCost(resourceType, amount)));
    }

    public Price GetCurrentPrice(int ownedAmount)
    {
        if (PriceList == null || PriceList.Count == 0)
            return null;
        if (ownedAmount < 0 || ownedAmount >= PriceList.Count)
            return null;
        return PriceList[ownedAmount];
    }

    public bool CanBuy(int ownedAmount)
    {
        var price = GetCurrentPrice(ownedAmount);
        if (price == null)
            return false;
        return price.ResourceCosts.All(resourceCost => GlobalManager.Values.Resources.HasResource(resourceCost.ResourceType, resourceCost.Amount));
    }

    public void SubtractResources(int ownedAmount)
    {
        var price = GetCurrentPrice(ownedAmount);
        if (price == null)
            return;
        foreach (var resourceCost in price.ResourceCosts)
            GlobalManager.Values.Resources.AddAmount(resourceCost.ResourceType, -resourceCost.Amount);
    }
}
