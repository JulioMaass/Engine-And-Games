using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.Graphics;
using Engine.Types;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.ECS.Components.VisualsHandling;

public class FrameSprite : Component
{
    public Texture2D Texture { get; private set; }
    public IntVector2 BorderSize { get; set; }

    public FrameSprite(Entity owner, string textureName, IntVector2 borderSize)
    {
        Owner = owner;
        Texture = Drawer.TextureDictionary[textureName];
        BorderSize = borderSize;
    }

    public void Draw()
    {
        var sourceRectangle = new IntRectangle(0, 0, Texture.Width, Texture.Height);
        var position = Owner.Position.Pixel - Owner.Sprite.Origin - BorderSize;
        var size = Owner.Sprite.FinalSize + BorderSize * 2;
        Drawer.DrawNineSliceTextureAt(Texture, sourceRectangle, position, size, BorderSize);
    }
}
