using Engine.Helpers;
using Engine.Main;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace Engine.Types;

public struct IntRectangle
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Left => X;
    public int Top => Y;
    public int Right => X + Width - 1; // For int collision calculation
    public int Bottom => Y + Height - 1;
    public int DrawingRight => X + Width; // For drawing purposes, to include the last pixel
    public int DrawingBottom => Y + Height;

    public IntVector2 Position
    {
        get => IntVector2.New(X, Y);
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }

    public IntVector2 Size
    {
        get => IntVector2.New(Width, Height);
        set
        {
            Width = value.X;
            Height = value.Y;
        }
    }

    public static IntRectangle Empty => new();

    public IntRectangle(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public IntRectangle(IntVector2 position, int width, int height)
        : this(position.X, position.Y, width, height) { }

    public IntRectangle(int x, int y, IntVector2 size)
        : this(x, y, size.X, size.Y) { }

    public IntRectangle(IntVector2 position, IntVector2 size)
        : this(position.X, position.Y, size.X, size.Y) { }

    public static bool operator ==(IntRectangle a, IntRectangle b)
    {
        return a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;
    }

    public static bool operator !=(IntRectangle a, IntRectangle b) => !(a == b);

    public bool Contains(IntVector2 position) =>
        X <= position.X && position.X < X + Width && Y <= position.Y && position.Y < Y + Height;

    public bool Overlaps(IntRectangle rectangle) =>
        Left <= rectangle.Right && Right >= rectangle.Left && Top <= rectangle.Bottom && Bottom >= rectangle.Top;

    public bool VerticallyOverlaps(IntRectangle rectangle) =>
        Top <= rectangle.Bottom && Bottom >= rectangle.Top;

    public bool HorizontallyOverlaps(IntRectangle rectangle) =>
        Left <= rectangle.Right && Right >= rectangle.Left;

    private bool IsAbove(IntRectangle rectangle) =>
        Bottom < rectangle.Top;

    private bool IsBelow(IntRectangle rectangle) =>
        Top > rectangle.Bottom;

    private bool IsLeftOf(IntRectangle rectangle) =>
        Right < rectangle.Left;

    private bool IsRightOf(IntRectangle rectangle) =>
        Left > rectangle.Right;

    public bool IsAboveAndAligned(IntRectangle rectangle) =>
        IsAbove(rectangle) && HorizontallyOverlaps(rectangle);

    public bool IsBelowAndAligned(IntRectangle rectangle) =>
        IsBelow(rectangle) && HorizontallyOverlaps(rectangle);

    public bool IsLeftOfAndAligned(IntRectangle rectangle) =>
        IsLeftOf(rectangle) && VerticallyOverlaps(rectangle);

    public bool IsRightOfAndAligned(IntRectangle rectangle) =>
        IsRightOf(rectangle) && VerticallyOverlaps(rectangle);


    public IntRectangle RoundDownToTileCoordinate()
    {
        var x1 = Left.RoundDownDivision(Settings.TileSize.X);
        var y1 = Top.RoundDownDivision(Settings.TileSize.Y);
        var x2 = Right.RoundDownDivision(Settings.TileSize.X);
        var y2 = Bottom.RoundDownDivision(Settings.TileSize.Y);
        var position = IntVector2.New(x1, y1);
        var size = IntVector2.New(x2 - x1 + 1, y2 - y1 + 1);
        return new IntRectangle(position, size);
    }

    public IntRectangle OffsetPosition(IntVector2 positionOffset)
    {
        Position += positionOffset;
        return this;
    }

    public IntVector2 DistanceToGetInside(IntRectangle rectangle)
    {
        var clampedRectangle = ClampInto(rectangle);
        return clampedRectangle.Position - Position;
    }

    public IntRectangle ClampInto(IntRectangle rectangle)
    {
        var x = Math.Clamp(X, rectangle.X, rectangle.X + rectangle.Width - Width);
        var y = Math.Clamp(Y, rectangle.Y, rectangle.Y + rectangle.Height - Height);
        return new IntRectangle(x, y, Width, Height);
    }

    public IntVector2 GetEdgePosition(IntVector2 dir)
    {
        if (dir.X == 0 || dir.Y == 0) // Values should be 1 or -1
            Debugger.Break();

        var edgeX = dir.X == 1 ? Right : Left;
        var edgeY = dir.Y == 1 ? Bottom : Top;
        return IntVector2.New(edgeX, edgeY);
    }

    public override bool Equals(object obj) // Added to fix compile warning
    {
        if (obj is IntRectangle rectangle)
            return this == rectangle;
        return false;
    }
    public override int GetHashCode() // Added to fix compile warning
    {
        return X.GetHashCode() ^ Y.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
    }

    public static implicit operator Rectangle(IntRectangle intRectangle)
    {
        return new Rectangle(intRectangle.X, intRectangle.Y, intRectangle.Width, intRectangle.Height);
    }

    public static implicit operator IntRectangle(Rectangle rectangle)
    {
        return new IntRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
    }

    public static IntRectangle GetRectangleCombination(IntRectangle intRectangle1, IntRectangle intRectangle2)
    {
        // Get the smallest rectangle that contains both rectangles
        var x1 = Math.Min(intRectangle1.Left, intRectangle2.Left);
        var y1 = Math.Min(intRectangle1.Top, intRectangle2.Top);
        var x2 = Math.Max(intRectangle1.Right, intRectangle2.Right);
        var y2 = Math.Max(intRectangle1.Bottom, intRectangle2.Bottom);
        return new IntRectangle(x1, y1, x2 - x1 + 1, y2 - y1 + 1);
    }
}