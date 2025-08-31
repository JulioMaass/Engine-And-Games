using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;

namespace Engine.ECS.Components;

public abstract class AngleComponent : Component
{
    public Angle Angle { get; set; }

    public void AddSpeedTowardsAngle(float speed, bool accelerateX, bool accelerateY)
    {
        if (accelerateX)
            AddXSpeedTowardsAngle(speed);
        if (accelerateY)
            AddYSpeedTowardsAngle(speed);
    }

    private void AddXSpeedTowardsAngle(float speed)
    {
        var xSpeed = Owner.Speed.X + GetXLength() * speed;
        Owner.Speed.SetXSpeed(xSpeed);
    }

    private void AddYSpeedTowardsAngle(float speed)
    {
        var ySpeed = Owner.Speed.Y + GetYLength() * speed;
        Owner.Speed.SetYSpeed(ySpeed);
    }

    public void ClampSpeedToAngle(float maxSpeed)
    {
        maxSpeed = Math.Min(maxSpeed, Owner.Speed.MaxSpeed);

        var angle = Angle.GetAngleFromDistanceCoordinates(Owner.Speed.Value);
        var maxXSpeed = Math.Abs(angle.GetXLength()) * maxSpeed;
        var maxYSpeed = Math.Abs(angle.GetYLength()) * maxSpeed;
        var xSpeed = MathHelper.Clamp(Owner.Speed.X, -maxXSpeed, maxXSpeed);
        var ySpeed = MathHelper.Clamp(Owner.Speed.Y, -maxYSpeed, maxYSpeed);
        Owner.Speed.SetSpeed(xSpeed, ySpeed);
    }

    public void SetAngleDirectionTo(Entity target)
    {
        if (target == null)
            return;

        Angle = Angle.GetDirection(Owner, target);
    }

    public void SetAngleDirectionToPosition(IntVector2 position)
    {
        Angle = Angle.GetDirection(Owner, position);
    }

    public void SetAngleDirectionToTile(IntVector2 target)
    {
        Angle = Angle.GetDirectionToTile(Owner, target);
    }

    public void TurnTowards(Entity target)
    {
        if (target == null)
            return;
        var targetAngle = Angle.GetDirection(Owner, target);
        TurnTowardsAngle(targetAngle.Value);
    }

    public void TurnTowardsAngle(int intendedAngle)
    {
        Angle = Angle.GetAngleTurnedTo(intendedAngle, Owner.Speed.TurnSpeed);
    }

    public void TurnTowardsAngle(int intendedAngle, int turnSpeed)
    {
        Angle = Angle.GetAngleTurnedTo(intendedAngle, turnSpeed);
    }

    public Vector2 GetVectorLength()
    {
        return Angle.GetVectorLength();
    }

    public float GetXLength()
    {
        return Angle.GetXLength();
    }

    public float GetYLength()
    {
        return Angle.GetYLength();
    }

    public void MirrorX()
    {
        Angle = Angle.GetMirrorX();
    }

    public void MirrorY()
    {
        Angle = Angle.GetMirrorY();
    }

    public Angle GetRoundedAngle(int possibleAngles, int angleOffset = 0)
    {
        return Angle.GetRoundedAngle(possibleAngles, angleOffset);
    }

    public void RoundAngle(int possibleAngles, int angleOffset = 0)
    {
        Angle = GetRoundedAngle(possibleAngles, angleOffset).Value;
    }

    public int GetRoundedIndex(int possibleAngles, int angleOffset = 0)
    {
        return Angle.GetRoundedIndex(possibleAngles, angleOffset);
    }
}
