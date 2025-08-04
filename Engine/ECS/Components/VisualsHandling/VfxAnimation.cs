using System;
using System.Collections.Generic;
using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Microsoft.Xna.Framework;

namespace Engine.ECS.Components.VisualsHandling;

public class VfxAnimation : Component
{
    // Size animation
    public Vector2 CurrentSize { get; set; } // float to allow subpixel speed
    private Vector2 ResizeAcceleration { get; set; }
    private Vector2 ResizeSpeed { get; set; }

    // Color animation
    public List<Color> Colors { get; } = new();
    public int ColorIndex { get; set; }

    public VfxAnimation(Entity owner)
    {
        Owner = owner;
    }

    public void Update()
    {
        // Size update
        Owner.Sprite.StretchedSize = CurrentSize;
        CurrentSize += ResizeSpeed;
        ResizeSpeed += ResizeAcceleration;

        // Color update
        //var index = ColorIndex % Colors.Count; // loop
        var index = Math.Min(ColorIndex, Colors.Count - 1); // stop at last color
        Owner.Sprite.Color = Colors[index];
        ColorIndex++;

        // Delete if size <= 0
        if (!(CurrentSize.X <= 0) && !(CurrentSize.Y <= 0)) 
            return;
        EntityManager.DeleteEntity(Owner);
    }

    public void SetInitialSize(int size)
    {
        CurrentSize = new Vector2(size, size);
    }

    public void SetSpeed(float resizeSpeed)
    {
        ResizeSpeed = new Vector2(resizeSpeed, resizeSpeed);
    }

    public void SetAcceleration(float acceleration)
    {
        ResizeAcceleration = new Vector2(acceleration, acceleration);
    }

    public void SetColors(int speed, params Color[] colors)
    {
        Colors.Clear();
        foreach (var c in colors)
            for (var i = 0; i < speed; i++)
                Colors.Add(c);
    }
}
