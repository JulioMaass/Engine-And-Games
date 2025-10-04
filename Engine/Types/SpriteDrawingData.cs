using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Types;

public struct SpriteDrawingData // Allows to pass sprite drawing data, so we can make draw calls outside the Sprite component
{
    public Texture2D Texture;
    public Rectangle DestinationRectangle;
    public Rectangle SourceRectangle;
    public Color Color;
    public float Rotation;
    public Vector2 Origin;
    public SpriteEffects Effects;
    public float Depth;

    public SpriteDrawingData(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color, float rotation,
        Vector2 origin, SpriteEffects effects, float depth)
    {
        Texture = texture;
        DestinationRectangle = destinationRectangle;
        SourceRectangle = sourceRectangle;
        Color = color;
        Rotation = rotation;
        Origin = origin;
        Effects = effects;
        Depth = depth;
    }
}
