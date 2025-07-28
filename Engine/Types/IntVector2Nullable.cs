namespace Engine.Types;

public class IntVector2Nullable
{
    public IntVector2Nullable(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }

    public static IntVector2Nullable New(int x, int y)
    {
        if (x == 0 && y == 0)
            return null;
        return new IntVector2Nullable(x, y);
    }

    public static IntVector2 ToIntVector2(IntVector2Nullable intVector2)
    {
        return intVector2 != null
            ? new IntVector2(intVector2.X, intVector2.Y)
            : IntVector2.Zero;
    }
}

