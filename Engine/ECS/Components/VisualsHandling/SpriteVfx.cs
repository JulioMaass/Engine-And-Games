using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Managers.Graphics;
using Engine.Types;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace Engine.ECS.Components.VisualsHandling;

public class SpriteVfx : Component
{
    public Texture2D Texture { get; private set; }
    public Color Color { get; set; } = CustomColor.White;
    public IntVector2 Size { get; set; }
    public IntVector2 Origin { get; set; }
    public float Alpha { get; set; } = 1.0f;

    public SpriteVfx(Entity owner, string textureName, IntVector2 size)
    {
        Owner = owner;
        Texture = Drawer.TextureDictionary[textureName];
        Size = size;
        Origin = Size / 2;
    }

    public void Draw()
    {
        var size = Texture.GetSize();
        var sourceRectangle = new IntRectangle(IntVector2.Zero, size);
        var position = Owner.Position.Pixel - Size / 2;
        var destinationRectangle = new IntRectangle(position, Size);
        var color = CustomColor.ColorWithAlpha(Color, Alpha);
        Video.SpriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, color);
    }
}