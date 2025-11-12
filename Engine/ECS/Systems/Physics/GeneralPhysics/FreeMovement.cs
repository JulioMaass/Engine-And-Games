using Engine.ECS.Components;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using Microsoft.Xna.Framework;

namespace Engine.ECS.Systems.Physics.GeneralPhysics;

public class FreeMovement : Component
{
    private IntVector2 Pixel => Owner.Position.Pixel;
    private Vector2 Fraction => Owner.Position.Fraction;
    public void SetPixelX(int x) => Owner.Position.Pixel = Owner.Position.Pixel with { X = x };
    public void SetPixelY(int y) => Owner.Position.Pixel = Owner.Position.Pixel with { Y = y };
    public void SetFractionX(float xFraction) => Owner.Position.Fraction = Owner.Position.Fraction with { X = xFraction };
    public void SetFractionY(float yFraction) => Owner.Position.Fraction = Owner.Position.Fraction with { Y = yFraction };

    public FreeMovement(Entity owner)
    {
        Owner = owner;
    }

    public void MoveInPixelsAndFraction()
    {
        MoveInPixelsAndFraction(Owner.Speed.X, Owner.Speed.Y);
    }

    public void MoveInPixelsAndFraction(float xSpeed, float ySpeed) // moves both X and Y at once for performance
    {
        var xPixelDestiny = Pixel.X;
        var yPixelDestiny = Pixel.Y;

        if (xSpeed != 0)
        {
            (var xFloatDestiny, xPixelDestiny) = GetDestinyCoordinate(xSpeed, Pixel.X, Fraction.X);
            SetFractionX(xFloatDestiny - xPixelDestiny);
        }
        if (ySpeed != 0)
        {
            (var yFloatDestiny, yPixelDestiny) = GetDestinyCoordinate(ySpeed, Pixel.Y, Fraction.Y);
            SetFractionY(yFloatDestiny - yPixelDestiny);
        }
        Owner.Position.Pixel = new IntVector2(xPixelDestiny, yPixelDestiny);
    }

    public void MoveXInPixelsAndFraction(float xSpeed)
    {
        if (xSpeed == 0)
            return;

        var (xFloatDestiny, xPixelDestiny) = GetDestinyCoordinate(xSpeed, Pixel.X, Fraction.X);
        SetFractionX(xFloatDestiny - xPixelDestiny);
        SetPixelX(xPixelDestiny);
    }

    public void MoveYInPixelsAndFraction(float ySpeed)
    {
        if (ySpeed == 0)
            return;

        var (yFloatDestiny, yPixelDestiny) = GetDestinyCoordinate(ySpeed, Pixel.Y, Fraction.Y);
        SetFractionY(yFloatDestiny - yPixelDestiny);
        SetPixelY(yPixelDestiny);
    }

    private static (float, int) GetDestinyCoordinate(float speed, int pixelCoordinate, float fraction)
    {
        var floatDestiny = pixelCoordinate + speed + fraction;
        int pixelDestiny;
        if (speed % 1 == 0)
            pixelDestiny = pixelCoordinate + (int)speed; // guarantees to move at least 1 pixel, even when going from .5 to another .5 coordinate and changing direction
        else
            pixelDestiny = PhysicsHelperFunctions.GetPixelDestiny(floatDestiny, speed); // using this method makes entity go back at 0.5 fraction

        return (floatDestiny, pixelDestiny);
    }
}
