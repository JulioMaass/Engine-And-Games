using Engine.ECS.Components;
using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Types;
using System;

namespace Engine.ECS.Systems.Physics.SpecialMovement;

// TODO: Choose if parabolic movement hits on going or coming back, on each function
public class ParabolicMovement : Component
{
    public ParabolicMovement(Entity owner)
    {
        Owner = owner;
    }

    private (Axis axis, int sign, int escapeDistance, int sidewaysDistance, float gravity) GetCalculationData(Entity target)
    {
        var distance = target.Position.Pixel - Owner.Position.Pixel;
        var (axis, sign) = Owner.Gravity.GetGravityAxisAndSign();
        var (escapeDistance, sidewaysDistance) = ConvertDistanceToNewAxis(distance, axis);
        var gravity = Owner.Gravity.Force * sign;
        return (axis, sign, escapeDistance, sidewaysDistance, gravity);
    }

    public void LaunchAtEntityAtTime(Entity target, int frames)
    {
        if (target == null)
            return;
        var (axis, sign, escapeDistance, sidewaysDistance, gravity) =
            GetCalculationData(target);

        var sidewaysSpeed = (float)sidewaysDistance / frames;
        var escapeSpeed = (escapeDistance - 0.5f * gravity * frames * frames) / frames;

        ApplySpeeds(escapeSpeed, sidewaysSpeed, axis);
    }

    public void LaunchAtEntityWithSidewaysSpeed(Entity target, float sidewaysSpeed)
    {
        if (target == null)
            return;
        var (axis, sign, escapeDistance, sidewaysDistance, gravity) =
            GetCalculationData(target);

        var time = Math.Abs(sidewaysDistance) / sidewaysSpeed;
        sidewaysSpeed *= Math.Sign(sidewaysDistance);
        var escapeSpeed = (escapeDistance - 0.5f * gravity * time * time) / time;

        ApplySpeeds(escapeSpeed, sidewaysSpeed, axis);
    }

    public void LaunchAtEntityWithEscapeSpeed(Entity target, float escapeSpeed)
    {
        if (target == null)
            return;
        var (axis, sign, escapeDistance, sidewaysDistance, gravity) =
            GetCalculationData(target);

        escapeSpeed *= -sign;
        var timeToPeak = Math.Abs(escapeSpeed / gravity);
        var peakDistance = escapeSpeed * timeToPeak + 0.5f * gravity * timeToPeak * timeToPeak;
        var totalEscapeDistance = peakDistance - escapeDistance;
        var timeToReturn = (float)Math.Sqrt(2 * Math.Abs(totalEscapeDistance) / Math.Abs(gravity));
        if (float.IsNaN(timeToReturn)) // If target is unreachable, top of the arc will align with target
            timeToReturn = 0;
        var totalTime = timeToPeak + timeToReturn;
        var sidewaysSpeed = sidewaysDistance / totalTime;

        ApplySpeeds(escapeSpeed, sidewaysSpeed, axis);
    }

    public void LaunchAtEntityWithAngleSpeed(Entity target, float speed)
    {
        if (target == null)
            return;
        var (axis, sign, escapeDistance, sidewaysDistance, gravity) =
            GetCalculationData(target);

        var s = speed;
        var x = -Math.Abs(sidewaysDistance);
        var y = -escapeDistance;
        var g = gravity;

        double sqrt = s * s * s * s - g * (g * (x * x) + 2 * y * (s * s));
        sqrt = Math.Sqrt(sqrt);
        var angleInRadians = Math.Atan((s * s + sqrt) / (g * x));
        var angle = (int)(angleInRadians * 180000 / Math.PI);
        angle = Angle.TurnDegreesPositive(angle);

        // fix angle for horizontal gravity
        if (axis == Axis.X)
        {
            angle = -angle - 90000;
            if (sidewaysDistance > 0)
                angle += 180000;
        }
        // fix it always shooting to one side (mirror half the angles)
        if (sidewaysDistance < 0)
            angle = (360000 - angle + 180000) % 360000;
        // fix being aligned in the escape axis sometimes make it aim straight to target, instead of opposite
        if (Owner.Gravity.GravityDir == Dir.Down && angle == 90000)
            angle = 270000;
        if (Owner.Gravity.GravityDir == Dir.Up && angle == 270000)
            angle = 90000;

        // when out of reach
        if (double.IsNaN(sqrt))
        {
            var escapeAngle = axis == Axis.X ? 180000 : 270000; // TODO: Need to have 4 directions?
            escapeAngle = sign > 0 ? escapeAngle : (escapeAngle + 180000) % 360000;
            var directAngle = Angle.GetDirection(Owner, target).Value;
            var differenceToEscapeAngle = Angle.TurnDegreesPositive(directAngle - escapeAngle);
            var zeroIsEscape = (differenceToEscapeAngle + 180000) % 360000 - 180000; // goes from -180 to 180
            angle = Angle.TurnDegreesPositive(directAngle - zeroIsEscape / 3);
        }

        // Convert angle to axis-agnostic speeds
        var angleXSpeed = Angle.GetXLength(angle) * s;
        var angleYSpeed = Angle.GetYLength(angle) * s;
        ApplySpeeds(
            escapeSpeed: axis == Axis.X ? angleXSpeed : angleYSpeed,
            sidewaysSpeed: axis == Axis.X ? angleYSpeed : angleXSpeed,
            axis
        );
    }

    private static (int escapeDistance, int sidewaysDistance) ConvertDistanceToNewAxis(IntVector2 distance, Axis gravityAxis)
    {
        return gravityAxis == Axis.X
            ? (distance.X, distance.Y)
            : (distance.Y, distance.X);
    }

    private void ApplySpeeds(float escapeSpeed, float sidewaysSpeed, Axis gravityAxis)
    {
        if (gravityAxis == Axis.X)
        {
            Owner.Speed.SetXSpeed(escapeSpeed);
            Owner.Speed.SetYSpeed(sidewaysSpeed);
        }
        else
        {
            Owner.Speed.SetXSpeed(sidewaysSpeed);
            Owner.Speed.SetYSpeed(escapeSpeed);
        }
    }
}
