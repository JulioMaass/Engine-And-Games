using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Engine.ECS.Components.VisualsHandling;

public class Palette : Component
{
    private Texture2D Texture { get; }
    private int Index { get; set; }
    public List<Func<bool>> Conditions { get; } = new();
    public List<List<int>> Patterns { get; } = new(); // like 0 1 2 1 for charging shots
    public List<Func<int>> PatternsOrigin { get; } = new(); // the frame counter that feeds the patterns
    public List<int> PatternsSpeed { get; } = new();

    public Palette(Entity owner, Texture2D texture)
    {
        Owner = owner;
        Texture = texture;
    }

    private int UpdateIndex()
    {
        if (Conditions.Count == 0)
            return 0;
        for (var i = 0; i < Conditions.Count; i++)
        {
            if (Conditions[i]())
            {
                var index = PatternsOrigin[i]() / PatternsSpeed[i] % Patterns[i].Count;
                return Patterns[i][index];
            }
        }
        return 0;
    }

    public void SetPalette()
    {
        Index = UpdateIndex();
        PaletteManager.SetPalette(Texture, Index);
    }
}
