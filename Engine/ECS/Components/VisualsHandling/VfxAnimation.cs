using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.Graphics;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.ECS.Components.VisualsHandling;

public class VfxAnimation : Component
{
    // Size animation
    public Vector2 CurrentSize { get; private set; } // float to allow subpixel speed
    private bool InitialSizeSet { get; set; }
    private Vector2 ResizeAcceleration { get; set; }
    private Vector2 ResizeSpeed { get; set; }
    public List<int> InnerSizeReduction { get; private set; } = new(); // Size reduction per simultaneous color

    // Color animation
    public List<List<Color>> Colors { get; } = new(); // Inner list is simultaneous colors, outer list is the sequence
    private int ColorFrame { get; set; }
    public int ColorIndex { get; private set; }
    public int ColorSpeed { get; private set; }
    public bool LoopColors { get; set; }

    // Trail animation
    public int TrailFrames { get; set; }
    public List<IntVector2> TrailPositions { get; } = new();
    public float TrailFrameResize { get; set; } = -0.5f;
    public Vector2 TrailFrameSize { get; set; }

    public VfxAnimation(Entity owner)
    {
        Owner = owner;
    }

    public void Update()
    {
        // Size update
        if (!InitialSizeSet)
            CurrentSize = Owner.Sprite.FinalSize;
        Owner.Sprite.StretchedSize = CurrentSize;
        CurrentSize += ResizeSpeed;
        ResizeSpeed += ResizeAcceleration;

        // Color update
        ColorIndex = ColorFrame / ColorSpeed;
        if (LoopColors)
            ColorIndex %= Colors.Count;
        else
            ColorIndex = Math.Min(ColorIndex, Colors.Count - 1);
        ColorFrame++;

        // Trail update
        TrailPositions.Add(Owner.Position.Pixel);
        if (TrailPositions.Count > TrailFrames + 1)
            TrailPositions.RemoveAt(0);

        // Delete if size <= 0
        if (CurrentSize.X <= 0 || CurrentSize.Y <= 0)
            EntityManager.MarkEntityForDeletion(Owner);
    }

    public void SetInitialSize(int size)
    {
        CurrentSize = new Vector2(size, size);
        InitialSizeSet = true;
    }

    public void SetSpeed(float resizeSpeed) =>
        ResizeSpeed = new Vector2(resizeSpeed, resizeSpeed);

    public void SetInnerSizeReduction(params int[] reductions) =>
        InnerSizeReduction = new List<int>(reductions);

    public void SetAcceleration(float acceleration) =>
        ResizeAcceleration = new Vector2(acceleration, acceleration);

    public void SetSingleColorAnimation(int speed, params Color[] colors) // has one color per frame
    {
        ColorSpeed = speed;
        Colors.Clear();
        foreach (var color in colors)
            Colors.Add([color]);
    }

    public void SetNestedColorAnimation(int speed, params List<Color>[] colors) // Has multiple colors per frame
    {
        ColorSpeed = speed;
        Colors.Clear();
        foreach (var colorList in colors)
        {
            Colors.Add(new List<Color>());
            foreach (var color in colorList)
                Colors[^1].Add(color);
        }
    }

    private List<IntVector2> GetLinkedTrailPositions()
    {
        var linkedPositions = new List<IntVector2>();
        var trail = new List<IntVector2> { TrailPositions[0], TrailPositions[^1] };

        for (var i = 0; i < trail.Count - 1; i++)
        {
            var start = trail[i];
            var end = trail[i + 1];
            var linePoints = GetBresenhamLine(start, end);
            linkedPositions.AddRange(linePoints);
        }
        return linkedPositions;
    }

    private List<IntVector2> GetBresenhamLine(IntVector2 start, IntVector2 end)
    {
        var points = new List<IntVector2>();

        var x = start.X;
        var y = start.Y;

        var xDistance = Math.Abs(end.X - x);
        var yDistance = Math.Abs(end.Y - y);
        var xStep = (x < end.X) ? 1 : -1;
        var yStep = (y < end.Y) ? 1 : -1;
        var err = xDistance - yDistance;

        while (true)
        {
            points.Add(new IntVector2(x, y));

            if (x == end.X && y == end.Y)
                break;

            var e2 = 2 * err;
            if (e2 > -yDistance)
            {
                err -= yDistance;
                x += xStep;
            }
            if (e2 < xDistance)
            {
                err += xDistance;
                y += yStep;
            }
        }
        return points;
    }

    public void Draw(SpriteDrawingData spriteDrawingData)
    {
        var linkedTrailPositions = GetLinkedTrailPositions();
        for (var i = 0; i < InnerSizeReduction.Count + 1; i++)
        {
            TrailFrameSize = CurrentSize;
            var reduction = i == 0 ? 0 : InnerSizeReduction[i - 1];
            for (var j = linkedTrailPositions.Count - 1; j >= 0; j--)
            {
                TrailFrameSize += new Vector2(TrailFrameResize, TrailFrameResize);
                var intTrailFrameSize = new IntVector2((int)TrailFrameSize.X, (int)TrailFrameSize.Y);

                var trailRectangle = new Rectangle(
                    linkedTrailPositions[j].X,
                    linkedTrailPositions[j].Y,
                    intTrailFrameSize.X - reduction * 2,
                    intTrailFrameSize.Y - reduction * 2
                    );

                if (trailRectangle.Width > 0 && trailRectangle.Height > 0)
                {
                    var color = Owner.Sprite.CalculateFinalColor(Colors[ColorIndex][i]);
                    Video.SpriteBatch.Draw(spriteDrawingData.Texture, trailRectangle, spriteDrawingData.SourceRectangle,
                        color, spriteDrawingData.Rotation, spriteDrawingData.Origin,
                        spriteDrawingData.Effects, spriteDrawingData.Depth);
                }
            }
        }
    }
}
