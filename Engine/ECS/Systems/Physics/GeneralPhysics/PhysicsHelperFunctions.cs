using Engine.ECS.Components;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;

namespace Engine.ECS.Systems.Physics.GeneralPhysics;

public class PhysicsHelperFunctions : Component
{
    private IntVector2 Pixel => Owner.Position.Pixel;
    private Vector2 Fraction => Owner.Position.Fraction;

    public PhysicsHelperFunctions(Entity owner)
    {
        Owner = owner;
    }

    // Round destiny to its closest integer (round back on 0.5)
    public static int GetPixelDestiny(double position, double direction)
    {
        int roundedNumber;
        if (direction > 0)
            roundedNumber = (int)Math.Ceiling(position - 0.5);
        else
            roundedNumber = (int)Math.Floor(position + 0.5);

        return roundedNumber;
    }

    // Round destiny to its max (negative speed rounds down, positive rounds up) - Used for solid collision testing
    // It avoids getting yFractions fluctuations while standing on a surface, for example
    public int GetMaxedDestinyX(float xSpeed)
    {
        return GetMaxedDestiny(Pixel.X, xSpeed, Fraction.X);
    }

    public int GetMaxedDestinyY(float ySpeed)
    {
        return GetMaxedDestiny(Pixel.Y, ySpeed, Fraction.Y);
    }

    private int GetMaxedDestiny(int position, double speed, double fraction)
    {
        if (speed == 0)
            return position;

        var destiny = position + speed + fraction;
        return GetMaxedIntegerFromMovement(destiny, speed);
    }

    private static int GetMaxedIntegerFromMovement(double position, double direction)
    {
        int roundedNumber;
        if (direction > 0)
            roundedNumber = (int)Math.Ceiling(position);
        else
            roundedNumber = (int)Math.Floor(position);

        return roundedNumber;
    }
}
