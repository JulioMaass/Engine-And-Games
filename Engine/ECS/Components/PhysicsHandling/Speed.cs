using Engine.ECS.Entities.EntityCreation;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Engine.ECS.Components.PhysicsHandling;

public class Speed : Component
{
    public Vector2 Value { get; private set; }
    public float X => Value.X;
    public float Y => Value.Y;
    public Vector2 Force { get; set; } // Used to keep speed even after hitting wall (ex.: jump and bump on a ledge)

    // Properties
    public float MoveSpeed { get; set; }
    public float JumpSpeed { get; set; }
    public float DashSpeed { get; set; }
    public float KnockbackSpeed { get; set; } = 1.25f;
    public int TurnSpeed { get; set; }
    public float MaxSpeed { get; set; } = 8;
    public float Acceleration { get; set; }
    public int CrawlTurnDirection { get; set; }

    public Speed(Entity owner)
    {
        Owner = owner;
    }

    public void SetSpeed(float xSpeed, float ySpeed)
    {
        var speed = new Vector2(xSpeed, ySpeed);
        SetSpeed(speed);
    }

    public void SetSpeed(Vector2 speed)
    {
        speed.X = MathHelper.Clamp(speed.X, -MaxSpeed, MaxSpeed);
        speed.Y = MathHelper.Clamp(speed.Y, -MaxSpeed, MaxSpeed);
        Value = speed;
    }

    public void SetXSpeed(float xSpeed)
    {
        xSpeed = MathHelper.Clamp(xSpeed, -MaxSpeed, MaxSpeed);
        Value = Value with { X = xSpeed };
    }

    public void SetYSpeed(float ySpeed)
    {
        ySpeed = MathHelper.Clamp(ySpeed, -MaxSpeed, MaxSpeed);
        Value = Value with { Y = ySpeed };
    }

    public void AddXSpeed(float xSpeed)
    {
        var newXSpeed = X + xSpeed;
        newXSpeed = MathHelper.Clamp(newXSpeed, -MaxSpeed, MaxSpeed);
        Value = Value with { X = newXSpeed };
    }

    public void AddYSpeed(float ySpeed)
    {
        var newYSpeed = Y + ySpeed;
        newYSpeed = MathHelper.Clamp(newYSpeed, -MaxSpeed, MaxSpeed);
        Value = Value with { Y = newYSpeed };
    }

    public void TurnClockwise()
    {
        Value = Value with { X = -Y, Y = X };
    }

    public void TurnCounterClockwise()
    {
        Value = Value with { X = Y, Y = -X };
    }

    public void SetMoveSpeedToCurrentDirection()
    {
        if (MoveSpeed == 0)
            Debugger.Break(); // No move speed was added
        SetSpeedToCurrentDirection(MoveSpeed);
    }

    public void SetMoveSpeedToCurrentDirection(float speed)
    {
        MoveSpeed = speed;
        SetSpeedToCurrentDirection(MoveSpeed);
    }

    public void SetSpeedToCurrentDirection(float speed)
    {
        speed = MathHelper.Clamp(speed, -MaxSpeed, MaxSpeed);
        SetXSpeed(Owner.MoveDirection.GetXLength() * speed);
        SetYSpeed(Owner.MoveDirection.GetYLength() * speed);
    }
}