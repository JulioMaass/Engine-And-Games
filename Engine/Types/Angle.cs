using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Main;
using Microsoft.Xna.Framework;
using System;

namespace Engine.Types;

public struct Angle
{
    private int _value;
    public int Value // 360.000 = 360 degrees
    {
        get => _value;
        set => _value = TurnDegreesPositive(value);
    }

    public Angle(int angle = 0)
    {
        _value = TurnDegreesPositive(angle);
    }

    public static implicit operator Angle(int value) => new(value); // Allows implicit conversion from int to Angle

    // Angle turner
    // Static
    private static Angle GetAngleTurnedTo(int currentAngle, int intendedAngle, int turnSpeed)
    {
        var angleDifference = TurnDegreesPositive(intendedAngle - currentAngle);

        if (Math.Abs(angleDifference) <= turnSpeed)
        {
            currentAngle = intendedAngle;
        }
        else
        {
            if (angleDifference < 180000)
                currentAngle += turnSpeed;
            else
                currentAngle -= turnSpeed;
        }
        return currentAngle;
    }
    // Instance
    public Angle GetAngleTurnedTo(int intendedAngle, int turnSpeed)
    {
        return GetAngleTurnedTo(Value, intendedAngle, turnSpeed);
    }

    // Direction between two entities
    public static Angle GetDirection(Entity origin, Entity target)
    {
        return target == null
            ? new Angle()
            : GetDirection(origin, target.Position.Pixel);
    }

    public static Angle GetDirection(Entity origin, IntVector2 targetPosition)
    {
        var dx = targetPosition.X - origin.Position.Pixel.X;
        var dy = targetPosition.Y - origin.Position.Pixel.Y;
        var distance = IntVector2.New(dx, dy);

        var angle = GetAngleFromDistanceCoordinates(distance);
        return angle;
    }

    // Direction to a tile
    // Static
    public static Angle GetDirectionToTile(Entity origin, IntVector2 target)
    {
        var dx = target.X * Settings.TileSize.X + Settings.TileSize.X / 2 - origin.Position.Pixel.X; // TODO: Make a helper function to convert tile to pixel/center
        var dy = target.Y * Settings.TileSize.Y + Settings.TileSize.Y / 2 - origin.Position.Pixel.Y;
        var distance = IntVector2.New(dx, dy);

        var angle = GetAngleFromDistanceCoordinates(distance);
        return angle;
    }

    // X length getter
    // Static
    public static float GetXLength(int angle)
    {
        return ((float)Math.Cos(angle * Math.PI / 180000)).RemoveDecimalsIfNegligible();
    }
    // Instance
    public float GetXLength()
    {
        return GetXLength(Value);
    }

    // Y length getter
    // Static
    public static float GetYLength(int angle)
    {
        return ((float)Math.Sin(angle * Math.PI / 180000)).RemoveDecimalsIfNegligible();
    }
    // Instance
    public float GetYLength()
    {
        return GetYLength(Value);
    }

    // Dir getter
    // Instance
    public int GetXDir()
    {
        return GetXLength().GetSign();
    }

    public int GetYDir()
    {
        return GetYLength().GetSign();
    }

    // Vector length getter (both X and Y)
    // Static
    public static Vector2 GetVectorLength(int angle)
    {
        return new(GetXLength(angle), GetYLength(angle));
    }
    // Instance
    public Vector2 GetVectorLength()
    {
        return GetVectorLength(Value);
    }

    // Mirror X
    // Static
    public static Angle MirrorX(int angle)
    {
        return (360000 - angle + 180000) % 360000;
    }
    // Instance
    public Angle MirrorX()
    {
        return MirrorX(Value);
    }
    // Mirror Y
    // Static
    public static Angle MirrorY(int angle)
    {
        return (360000 - angle) % 360000;
    }
    // Instance
    public Angle MirrorY()
    {
        return MirrorY(Value);
    }
    // Reverse angle
    // Static
    public static Angle Reverse(int angle)
    {
        return (angle + 180000) % 360000;
    }
    // Instance
    public Angle Reverse()
    {
        return Reverse(Value);
    }

    // Get angle from coordinates
    public static Angle GetAngleFromDistanceCoordinates(Vector2 distance)
    {
        var radiansAngle = Math.Atan2(distance.Y, distance.X);
        return RadianToDegrees(radiansAngle);
    }

    // Helper functions
    private static Angle RadianToDegrees(double radian)
    {
        var angle = (int)(radian * 180000 / Math.PI); // From -180 to 180
        return TurnDegreesPositive(angle);
    }
    public static int TurnDegreesPositive(int degrees)
    {
        return (degrees + 360000) % 360000; // From 0 to 360
    }
    private static double DegreesToRadian(int degrees)
    {
        return degrees * Math.PI / 180000;
    }

    public Angle GetRoundedAngle(int possibleAngles, int angleOffset = 0)
    {
        // ex: angle 345, possible angles 4, offset 30 (may return 30, 120, 210, 300)
        var stepSize = 360000f / possibleAngles; // 90
        var dirIndex = GetRoundedIndex(possibleAngles, angleOffset); // 0
        var indexedAngle = dirIndex * stepSize + angleOffset; // 0 * 90 + 30 = 30
        return (int)indexedAngle;
    }

    public int GetRoundedIndex(int possibleAngles, int angleOffset = 0)
    {
        // ex: angle 345, possible angles 4, offset 30 (may return 30, 120, 210, 300, with indexes 0, 1, 2, 3)
        var stepSize = 360000f / possibleAngles; // 90
        var offsetAngle = (Value + 360000 - angleOffset + stepSize / 2) % 360000; // 345 + 360 - 30 + 90 / 2 = 720 = 0
        return (int)Math.Floor(offsetAngle / stepSize); // 0 / 90 = 0
    }
}
