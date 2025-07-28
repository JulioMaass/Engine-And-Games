using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;

namespace Engine.ECS.Components.VisualsHandling;

public class Tweening : Component
{
    private IntVector2 StartingPosition { get; set; }
    private Vector2 CurrentPosition { get; set; }
    private IntVector2 FinalPosition { get; set; }
    public Vector2 LastFrameMovement { get; private set; }

    private bool IsTweening { get; set; }

    private int Duration { get; set; }
    private int Frame { get; set; }
    private TweeningType Type { get; set; }

    private float Gravity { get; set; }
    private float ZSpeed { get; set; }
    private float ZPosition { get; set; }

    public Tweening(Entity owner)
    {
        Owner = owner;
    }

    public void TweenTo(IntVector2 destiny, TweeningType type = TweeningType.Instant, int duration = 0)
    {
        IsTweening = true;

        StartingPosition = Owner.Position.Pixel;
        CurrentPosition = Owner.Position.Pixel;
        FinalPosition = destiny;

        Owner.Position.Pixel = destiny;

        Type = type;

        Frame = 0;
        Duration = duration;

        // Set up type-specific variables
        if (Type == TweeningType.Jump)
        {
            var distance = Math.Sqrt(Math.Pow(StartingPosition.X - FinalPosition.X, 2) + Math.Pow(StartingPosition.Y - FinalPosition.Y, 2));
            Gravity = (float)distance / 100.0f;
            ZSpeed = duration * Gravity / 2.0f;
            ZPosition = 0;
        }
    }

    public IntVector2 GetTweeningPosition()
    {
        if (IsTweening)
            return CurrentPosition;
        return Owner.Position.Pixel;
    }

    public void Update()
    {
        Frame++;
        var percentage = (float)Frame / Duration;
        var initialFramePosition = CurrentPosition;

        switch (Type)
        {
            case TweeningType.Linear:
                var x = Lerp(StartingPosition.X, FinalPosition.X, percentage);
                var y = Lerp(StartingPosition.Y, FinalPosition.Y, percentage);
                CurrentPosition = new(x, y);
                break;
            case TweeningType.Exponential:
                var remainingDistance = FinalPosition - CurrentPosition;
                CurrentPosition += remainingDistance / 3f;
                break;
            case TweeningType.Jump:
                ZPosition -= ZSpeed;
                ZSpeed -= Gravity;
                x = Lerp(StartingPosition.X, FinalPosition.X, percentage);
                y = Lerp(StartingPosition.Y, FinalPosition.Y, percentage) + ZPosition;
                CurrentPosition = new(x, y);
                break;
        }

        if (Owner.Position.Pixel == (IntVector2)CurrentPosition
            || Frame >= Duration)
            IsTweening = false;

        LastFrameMovement = CurrentPosition - initialFramePosition;
    }


    private float Lerp(int start, int end, float amount)
    {
        return MathHelper.Lerp(start, end, amount);
    }
}

public enum TweeningType
{
    Instant,
    Linear,
    Exponential,
    Jump
}
