using Engine.ECS.Entities.EntityCreation;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.ECS.Components.VisualsHandling;

public class SpriteVfxs : Component
{
    public List<SpriteVfx> List { get; } = new();

    public SpriteVfxs(Entity owner)
    {
        Owner = owner;
    }

    public void Add(string textureName, int size, Color color, float alpha)
    {
        var spriteVfx1 = new SpriteVfx(Owner, textureName, (size, size));
        spriteVfx1.Color = color;
        spriteVfx1.Alpha = alpha;
        List.Add(spriteVfx1);
    }

    public void Draw()
    {
        foreach (var spriteVfx in List)
            spriteVfx.Draw();
    }
}