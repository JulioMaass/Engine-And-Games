using Engine.Helpers;
using Engine.Main;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.Types;

public struct IntVector2
{
    public int X { get; set; }
    public int Y { get; set; }

    public int Width
    {
        get => X;
        set => X = value;
    }

    public int Height
    {
        get => Y;
        set => Y = value;
    }

    public IntVector2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static IntVector2 New(int x, int y) =>
        new(x, y);

    public static IntVector2 Square(int size) =>
        new(size, size);

    public static IntVector2 Zero => new(0, 0);
    public static IntVector2 PixelUp => new(0, -1);
    public static IntVector2 PixelDown => new(0, 1);
    public static IntVector2 PixelLeft => new(-1, 0);
    public static IntVector2 PixelRight => new(1, 0);

    // Operators
    public static bool operator ==(IntVector2 vector1, IntVector2 vector2)
    {
        return vector1.X == vector2.X && vector1.Y == vector2.Y;
    }
    public static bool operator !=(IntVector2 vector1, IntVector2 vector2)
    {
        return vector1.X != vector2.X || vector1.Y != vector2.Y;
    }
    public override bool Equals(object obj) // Added to fix compile warning
    {
        if (obj is IntVector2 vector)
            return this == vector;
        return false;
    }
    public override int GetHashCode() // Added to fix compile warning
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }
    public static IntVector2 operator -(IntVector2 vector1)
    {
        return New(-vector1.X, -vector1.Y);
    }
    public static IntVector2 operator -(IntVector2 vector1, IntVector2 vector2)
    {
        vector1.X -= vector2.X;
        vector1.Y -= vector2.Y;
        return vector1;
    }
    public static IntVector2 operator -(IntVector2 vector, int @int)
    {
        vector.X -= @int;
        vector.Y -= @int;
        return vector;
    }
    public static IntVector2 operator -(IntVector2 vector, (int X, int Y) tuple)
    {
        vector.X -= tuple.X;
        vector.Y -= tuple.Y;
        return vector;
    }
    public static Vector2 operator -(IntVector2 intVector, Vector2 vector)
    {
        var x = intVector.X - vector.X;
        var y = intVector.Y - vector.Y;
        return new Vector2(x, y);
    }
    public static IntVector2 operator +(IntVector2 vector1, IntVector2 vector2)
    {
        vector1.X += vector2.X;
        vector1.Y += vector2.Y;
        return vector1;
    }
    public static IntVector2 operator +(IntVector2 vector, (int X, int Y) tuple)
    {
        vector.X += tuple.X;
        vector.Y += tuple.Y;
        return vector;
    }
    public static IntVector2 operator +(IntVector2 vector, int @int)
    {
        vector.X += @int;
        vector.Y += @int;
        return vector;
    }
    public static Vector2 operator +(IntVector2 intVector, Vector2 vector)
    {
        var x = intVector.X + vector.X;
        var y = intVector.Y + vector.Y;
        return new Vector2(x, y);
    }
    public static IntVector2 operator *(IntVector2 vector, int @int)
    {
        vector.X *= @int;
        vector.Y *= @int;
        return vector;
    }
    public static Vector2 operator *(IntVector2 vector, float @float)
    {
        var x = vector.X * @float;
        var y = vector.Y * @float;
        return new Vector2(x, y);
    }
    public static IntVector2 operator *(IntVector2 vector1, IntVector2 vector2)
    {
        vector1.X *= vector2.X;
        vector1.Y *= vector2.Y;
        return vector1;
    }
    public static IntVector2 operator *(IntVector2 vector, (int X, int Y) tuple)
    {
        vector.X *= tuple.X;
        vector.Y *= tuple.Y;
        return vector;
    }
    public static IntVector2 operator /(IntVector2 vector1, IntVector2 vector2)
    {
        vector1.X /= vector2.X;
        vector1.Y /= vector2.Y;
        return vector1;
    }
    public static IntVector2 operator /(IntVector2 vector, int @int)
    {
        vector.X /= @int;
        vector.Y /= @int;
        return vector;
    }
    public static IntVector2 operator /(Point point, IntVector2 vector)
    {
        point.X /= vector.X;
        point.Y /= vector.Y;
        return point;
    }
    public static IntVector2 operator -(Point point, IntVector2 vector)
    {
        point.X -= vector.X;
        point.Y -= vector.Y;
        return point;
    }
    public static IntVector2 operator %(IntVector2 vector1, IntVector2 vector2)
    {
        vector1.X %= vector2.X;
        vector1.Y %= vector2.Y;
        return vector1;
    }
    public static implicit operator Vector2(IntVector2 intVector2)
    {
        return new Vector2(intVector2.X, intVector2.Y);
    }
    public static implicit operator Point(IntVector2 intVector2)
    {
        return new Point(intVector2.X, intVector2.Y);
    }
    public static implicit operator (int, int)(IntVector2 intVector2)
    {
        return (intVector2.X, intVector2.Y);
    }
    public static implicit operator IntVector2(Vector2 vector2)
    {
        return New(
            (int)Math.Round(vector2.X, MidpointRounding.AwayFromZero),
            (int)Math.Round(vector2.Y, MidpointRounding.AwayFromZero));
    }
    public static implicit operator IntVector2(Point point)
    {
        return New(point.X, point.Y);
    }
    public static implicit operator IntVector2((int X, int Y) tuple)
    {
        return New(tuple.X, tuple.Y);
    }

    public static IntVector2 Min(IntVector2 vector1, IntVector2 vector2)
    {
        return New(Math.Min(vector1.X, vector2.X), Math.Min(vector1.Y, vector2.Y));
    }

    public static IntVector2 Max(IntVector2 vector1, IntVector2 vector2)
    {
        return New(Math.Max(vector1.X, vector2.X), Math.Max(vector1.Y, vector2.Y));
    }

    public static IntVector2 Clamp(IntVector2 vectorMain, IntVector2 vectorMin, IntVector2 vectorMax)
    {
        vectorMain.X = MathHelper.Clamp(vectorMain.X, vectorMin.X, vectorMax.X);
        vectorMain.Y = MathHelper.Clamp(vectorMain.Y, vectorMin.Y, vectorMax.Y);
        return vectorMain;
    }

    public static IntVector2 TurnClockwise(IntVector2 intVector2)
    {
        return New(-intVector2.Y, intVector2.X);
    }

    public static IntVector2 TurnCounterClockwise(IntVector2 intVector2)
    {
        return New(intVector2.Y, -intVector2.X);
    }

    public static IntVector2 MirrorX(IntVector2 intVector2)
    {
        return New(-intVector2.X, intVector2.Y);
    }

    public IntVector2 MirrorX(bool doMirror = true)
    {
        return doMirror ? MirrorX(this) : this;
    }

    public IntVector2 RoundDownToTileSize()
    {
        return this / Settings.TileSize * Settings.TileSize;
    }

    public IntVector2 RoundDownToTileCoordinate() // Makes division of negative coordinates work as expected
    {
        var x = X.RoundDownDivision(Settings.TileSize.X);
        var y = Y.RoundDownDivision(Settings.TileSize.Y);
        return New(x, y);
    }

    public IntVector2 RoundDownDivision(IntVector2 divisor) // Makes division of negative coordinates work as expected
    {
        var x = X.RoundDownDivision(divisor.X);
        var y = Y.RoundDownDivision(divisor.Y);
        return New(x, y);
    }

    public List<IntVector2> GetNeighbors()
    {
        return new List<IntVector2>
        {
            new(X - 1, Y),
            new(X + 1, Y),
            new(X, Y - 1),
            new(X, Y + 1)
        };
    }

    public static int GetDistance(IntVector2 vector1, IntVector2 vector2)
    {
        return (int)Math.Sqrt(
            Math.Pow(vector1.X - vector2.X, 2) +
            Math.Pow(vector1.Y - vector2.Y, 2)
        );
    }

    public static IntVector2 GetDirFromAxis(Axis axis)
    {
        return axis switch
        {
            Axis.X => new IntVector2(1, 0),
            Axis.Y => new IntVector2(0, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null)
        };
    }
}
