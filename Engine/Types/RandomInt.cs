using Engine.Helpers;

namespace Engine.Types;

public class RandomInt
{
    public int Min { get; }
    public int Max { get; }
    public int Value { get; private set; }
    private int Spacing { get; set; } // TODO: Add spacing i.e. 20, 30, 40, etc.

    public bool RollsOnFalse { get; private set; }

    public RandomInt(int min, int max)
    {
        Min = min;
        Max = max;
        RollValue();
    }

    public static implicit operator RandomInt(int @int)
    {
        return new RandomInt(@int, @int);
    }

    public void RollValue()
    {
        Value = GetRandom.UnseededInt(Min, Max);
    }

    public RandomInt SetRollsOnFalse()
    {
        RollsOnFalse = true;
        return this;
    }

    private bool ReturnTrueAndCheckToRoll()
    {
        if (!RollsOnFalse)
            RollValue();
        return true;
    }

    private bool ReturnFalseAndCheckToRoll()
    {
        if (RollsOnFalse)
            RollValue();
        return false;
    }


    public static bool operator ==(RandomInt randomInt, int @int)
    {
        if (randomInt.Value == @int)
            return randomInt.ReturnTrueAndCheckToRoll();
        return randomInt.ReturnFalseAndCheckToRoll();
    }

    public static bool operator !=(RandomInt randomInt, int @int)
    {
        if (randomInt.Value != @int)
            return randomInt.ReturnTrueAndCheckToRoll();
        return randomInt.ReturnFalseAndCheckToRoll();
    }

    public static bool operator <(RandomInt randomInt, int @int)
    {
        if (randomInt.Value < @int)
            return randomInt.ReturnTrueAndCheckToRoll();
        return randomInt.ReturnFalseAndCheckToRoll();
    }

    public static bool operator >(RandomInt randomInt, int @int)
    {
        if (randomInt.Value > @int)
            return randomInt.ReturnTrueAndCheckToRoll();
        return randomInt.ReturnFalseAndCheckToRoll();
    }

    public static bool operator <=(RandomInt randomInt, int @int)
    {
        if (randomInt.Value <= @int)
            return randomInt.ReturnTrueAndCheckToRoll();
        return randomInt.ReturnFalseAndCheckToRoll();
    }

    public static bool operator >=(RandomInt randomInt, int @int)
    {
        if (randomInt.Value >= @int)
            return randomInt.ReturnTrueAndCheckToRoll();
        return randomInt.ReturnFalseAndCheckToRoll();
    }

    // Inverted operators
    public static bool operator ==(int @int, RandomInt randomInt) => randomInt == @int;
    public static bool operator !=(int @int, RandomInt randomInt) => randomInt != @int;
    public static bool operator <(int @int, RandomInt randomInt) => randomInt > @int;
    public static bool operator >(int @int, RandomInt randomInt) => randomInt < @int;
    public static bool operator <=(int @int, RandomInt randomInt) => randomInt >= @int;
    public static bool operator >=(int @int, RandomInt randomInt) => randomInt <= @int;
}
