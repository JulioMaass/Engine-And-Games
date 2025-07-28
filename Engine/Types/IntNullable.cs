namespace Engine.Types;

public class IntNullable
{
    public IntNullable(int value)
    {
        Value = value;
    }

    public int Value { get; set; }

    public static IntNullable New(int i)
    {
        if (i == 0)
            return null;
        return new IntNullable(i);
    }

    public static int ToInt(IntNullable i)
    {
        return i?.Value ?? 0;
    }
}