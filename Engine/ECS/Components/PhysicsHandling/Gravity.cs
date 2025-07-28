using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using System;

namespace Engine.ECS.Components.PhysicsHandling;

public class Gravity : Component
{
    public float Force { get; set; }
    public const float DEFAULT_FORCE = 0.25f; // TODO: Should be game specific
    private const int MAX_GRAVITY_SPEED = 8;
    public bool IsAffectedByGravity { get; set; } = true;
    public Dir GravityDir { get; set; } = Dir.Down;

    public Gravity(Entity owner, float force = DEFAULT_FORCE)
    {
        Owner = owner;
        Force = force;
    }

    public void Apply()
    {
        if (!IsAffectedByGravity)
            return;

        var (axis, dirSign) = GetGravityAxisAndSign();
        var currentSpeed = axis == Axis.X ? Owner.Speed.X : Owner.Speed.Y;
        var newSpeed = currentSpeed + Owner.Gravity.Force * dirSign;

        newSpeed = dirSign > 0
            ? Math.Min(newSpeed, MAX_GRAVITY_SPEED)
            : Math.Max(newSpeed, -MAX_GRAVITY_SPEED);

        if (axis == Axis.X)
            Owner.Speed.SetXSpeed(newSpeed);
        else
            Owner.Speed.SetYSpeed(newSpeed);
    }

    public (Axis axis, int sign) GetGravityAxisAndSign()
    {
        return GravityDir switch
        {
            Dir.Up => (Axis.Y, -1),
            Dir.Down => (Axis.Y, 1),
            Dir.Left => (Axis.X, -1),
            Dir.Right => (Axis.X, 1),
            Dir.Forward => (Axis.X, Owner.Facing.X >= 0 ? 1 : -1),
            Dir.Backward => (Axis.X, Owner.Facing.X >= 0 ? -1 : 1),
            _ => throw new ArgumentOutOfRangeException(nameof(GravityDir), GravityDir, null)
        };
    }
}