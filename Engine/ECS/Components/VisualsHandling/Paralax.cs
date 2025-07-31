using Engine.ECS.Entities.EntityCreation;
using Engine.Main;
using Engine.Managers.Graphics;
using Engine.Types;
using Microsoft.Xna.Framework;

namespace Engine.ECS.Components.VisualsHandling;

public class Paralax : Component
{
    // Properties
    public Vector2 DistancePercentage { get; set; } // 0 (no paralax) to 1 (full paralax - accompanies camera)
    public Vector2 Speed { get; set; }
    private Vector2 SpeedOffset { get; set; } // Distance traveled using paralax speed
    public ParalaxRepeat Repeat { get; set; }
    public IntVector2 RepeatDistance { get; set; }

    // Calculations
    private IntVector2 Spacing => Owner.Sprite.FinalSize + RepeatDistance;
    private IntVector2 FirstRepetitionPosition { get; set; } // Top left drawn sprite

    public Paralax(Entity owner)
    {
        Owner = owner;
    }

    public void Update()
    {
        SpeedOffset += Speed;
    }

    public IntVector2 GetParalaxOffset() // Calculates the position of the main sprite
    {
        // Camera.FractionalPanning is used to avoid paralax speed and camera being out of sync,
        // causing jitter (camera moves 1, 2, 1, 2, while paralax moves 2, 1, 2, 1).
        // Object would bounce left and right 1 pixel every frame
        var spawnScreenPosition = Owner.Position.StartingPosition / Settings.RoomSizeInPixels * Settings.RoomSizeInPixels;
        var distanceOffset = (Camera.FractionalPanning - (Vector2)spawnScreenPosition) * DistancePercentage;
        var panningRounding = Camera.Panning - Camera.FractionalPanning;
        return distanceOffset + SpeedOffset + panningRounding;
    }

    public void DrawRepetitions(IntRectangle sourceRectangle, IntVector2 position)
    {
        UpdateFirstRepetitionPosition(position);
        if (IsOutOfScreen())
            return;
        Draw(sourceRectangle);
    }

    private void UpdateFirstRepetitionPosition(IntVector2 position) // Calculates the position of the top left sprite
    {
        var distanceToEdge = position - Camera.Panning;
        var repetitionsToReachEdge = (Vector2)distanceToEdge / (Vector2)Spacing;
        var repetitionsSizeToEdge = Spacing * (IntVector2)repetitionsToReachEdge;
        var firstRepetitionPosition = position - repetitionsSizeToEdge - Spacing;
        if (Repeat == ParalaxRepeat.X)
            firstRepetitionPosition = (firstRepetitionPosition.X, position.Y);
        else if (Repeat == ParalaxRepeat.Y)
            firstRepetitionPosition = (position.X, firstRepetitionPosition.Y);
        FirstRepetitionPosition = firstRepetitionPosition;
    }

    private bool IsOutOfScreen()
    {
        if (Repeat == ParalaxRepeat.X)
        {
            if (FirstRepetitionPosition.Y - Spacing.Y > Camera.Panning.Y + Settings.ScreenSize.Height
                || FirstRepetitionPosition.Y + Spacing.Y < Camera.Panning.Y)
                return true;
        }
        else if (Repeat == ParalaxRepeat.Y)
        {
            if (FirstRepetitionPosition.X - Spacing.X > Camera.Panning.X + Settings.ScreenSize.Width
                || FirstRepetitionPosition.X + Spacing.X < Camera.Panning.X)
                return true;
        }
        return false;
    }

    private void Draw(IntRectangle sourceRectangle)
    {
        var flipSprite = Owner.Sprite.IsFlipped;
        var repetitionsAmount = GetRepetitionsAmount();
        for (var x = 0; x < repetitionsAmount.X; x++)
        {
            for (var y = 0; y < repetitionsAmount.Y; y++)
            {
                var repetitionPosition = FirstRepetitionPosition + new IntVector2(x * Spacing.X, y * Spacing.Y);
                Drawer.DrawTextureRectangleAt(Owner.Sprite.Texture, sourceRectangle, repetitionPosition, flipSprite, Owner.Sprite.Color);
            }
        }
    }

    private IntVector2 GetRepetitionsAmount()
    {
        var repetitionsAmount = Settings.ScreenSize / Spacing + 3;
        if (Repeat == ParalaxRepeat.X)
            repetitionsAmount = IntVector2.New(repetitionsAmount.X, 1);
        else if (Repeat == ParalaxRepeat.Y)
            repetitionsAmount = IntVector2.New(1, repetitionsAmount.Y);
        return repetitionsAmount;
    }
}

public enum ParalaxRepeat
{
    None,
    X,
    Y,
    Both
}