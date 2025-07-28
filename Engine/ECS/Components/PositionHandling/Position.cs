using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using Microsoft.Xna.Framework;

namespace Engine.ECS.Components.PositionHandling;

public class Position : Component
{
    public IntVector2 StartingPosition { get; set; }
    public IntVector2 Pixel { get; set; }
    public Vector2 Fraction { get; set; }

    public Position(Entity owner, int x, int y)
    {
        Owner = owner;
        Pixel = IntVector2.New(x, y);
    }

    public void SetPixelX(int x)
    {
        Pixel = IntVector2.New(x, Pixel.Y);
    }

    public void SetPixelY(int y)
    {
        Pixel = IntVector2.New(Pixel.X, y);
    }
}