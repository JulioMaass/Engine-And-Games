using System;

namespace Engine.Types;

public class Resource // Used to pass integers by reference
{
    public int Amount { get; set; }
    public int MaxAmount { get; private set; }
    public int MinAmount { get; }
    public ResourceType ResourceType { get; private set; }

    private static Resource NewEmpty(ResourceType resource, int maxValue)
    {
        var container = new Resource();
        container.ResourceType = resource;
        container.MaxAmount = maxValue;
        return container;
    }

    public static Resource NewFull(ResourceType resource, int maxValue)
    {
        var container = NewEmpty(resource, maxValue);
        container.Amount = maxValue;
        return container;
    }

    public static Resource New(ResourceType resource, int maxValue, int amount)
    {
        var container = NewEmpty(resource, maxValue);
        container.Amount = amount;
        return container;
    }

    public void Add(int value = 1)
    {
        Amount = Math.Clamp(Amount + value, MinAmount, MaxAmount);
    }
}

public enum ResourceType
{
    // Universal
    Hp,
    Score,
    // MMDB
    Ammo,
    Bolts,
    // Mole
    Bombs,
    Gold,
    // Candle
    Wax,
    // Shooter
    MachineGunAmmo,
    // SpaceMiner
    OreGray,
    OreRed,
    OreGreen,
    OreBlue,
    OreYellow,
}