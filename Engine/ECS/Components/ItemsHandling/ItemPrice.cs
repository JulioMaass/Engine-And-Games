using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.GlobalManagement;
using Engine.Types;
using System.Collections.Generic;
using System.Linq;

namespace Engine.ECS.Components.ItemsHandling;

public class ItemPrice : Component
{
    public List<Price> PriceList { get; } = []; // index 0 is price to buy, later indexes are upgrades
    public PriceType PriceType { get; set; }

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

    public void AddUnlockPrice(params (ResourceType, int)[] resourceCosts)
    {
        PriceType = PriceType.Unlock;
        var price = new Price();
        foreach (var (resourceType, amount) in resourceCosts)
            price.ResourceCosts.Add(new ResourceCost(resourceType, amount));
        PriceList.Add(price);
    }

    public void AddBuyPrices(ResourceType resourceType, params int[] resourceCosts)
    {
        PriceType = PriceType.Buy;
        foreach (var amount in resourceCosts)
            PriceList.Add(new Price(new ResourceCost(resourceType, amount)));
    }

    public void AddUpgradePrices(ResourceType resourceType1, ResourceType resourceType2, params (int, int)[] resourceCosts)
    {
        PriceType = PriceType.Upgrade;
        foreach (var (amount1, amount2) in resourceCosts)
        {
            var price = new Price();
            price.ResourceCosts.Add(new ResourceCost(resourceType1, amount1));
            price.ResourceCosts.Add(new ResourceCost(resourceType2, amount2));
            PriceList.Add(price);
        }
    }

    public Price GetCurrentPrice(int ownedAmount)
    {
        if (PriceList == null || PriceList.Count == 0 || ownedAmount < 0)
            return null;
        if (PriceList.Count == 1)
            return PriceList[0];
        if (ownedAmount >= PriceList.Count)
            return null;
        return PriceList[ownedAmount];
    }

    public bool CanBuy(int ownedAmount)
    {
        var price = GetCurrentPrice(ownedAmount);
        var unlocked = ownedAmount > 0 && PriceType == PriceType.Unlock;
        if (price == null || unlocked)
            return false;
        return price.ResourceCosts.All(resourceCost => GlobalManager.Values.MainCharData.Resources.HasResource(resourceCost.ResourceType, resourceCost.Amount));
    }

    public void SubtractResources(int ownedAmount)
    {
        var price = GetCurrentPrice(ownedAmount);
        if (price == null)
            return;
        foreach (var resourceCost in price.ResourceCosts)
            GlobalManager.Values.MainCharData.Resources.AddAmount(resourceCost.ResourceType, -resourceCost.Amount);
    }
}

public enum PriceType
{
    None,
    Buy,
    Upgrade,
    Unlock
}