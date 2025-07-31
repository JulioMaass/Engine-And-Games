using System;

namespace Engine.Helpers;

public static class GetRandom // TODO: Should have an option inside the GameSpecificOptions/GlobalValues to choose if all randoms are seeded or not
{
    private static readonly Random SeededRandom = new(1);
    private static readonly Random UnseededRandom = new();

    public static int SeededInt(int @int) // 0 to _int - 1
    {
        return SeededRandom.Next(@int);
    }

    public static int SeededInt(int min, int max) // _min to _max inclusive
    {
        max++;
        return SeededRandom.Next(min, max);
    }

    public static int UnseededInt(int @int) // 0 to _int - 1
    {
        return UnseededRandom.Next(@int);
    }

    public static int UnseededInt(int min, int max) // _min to _max inclusive
    {
        max++;
        return UnseededRandom.Next(min, max);
    }

    public static float UnseededFloat(float max) // 0.0f to _max
    {
        return (float)UnseededRandom.NextDouble() * max;
    }

    public static float UnseededFloat(float min, float max) // _min to _max
    {
        return (float)UnseededRandom.NextDouble() * (max - min) + min;
    }

    public static bool UnseededBool() // true or false
    {
        return UnseededRandom.Next(2) == 0;
    }
}