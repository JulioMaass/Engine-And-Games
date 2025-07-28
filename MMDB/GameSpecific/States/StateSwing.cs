using Engine.ECS.Components.ControlHandling.States;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;

namespace MMDB.GameSpecific.States;

public class StateSwing : State
{
    private IntVector2 SwingPosition { get; set; }
    private float Angle { get; set; } = 180000;
    private float AngleSpeed { get; set; }
    private int Distance => 48;

    public override bool StartCondition()
    {
        return true;
    }

    public override bool KeepCondition()
    {
        return true;
    }

    public override bool PostProcessingKeepCondition()
    {
        return true;
    }

    public override void StateSettingBehavior()
    {
        SwingPosition = Owner.Position.Pixel + (Distance, 0);
    }

    public override void Behavior()
    {
        var angleInRadians = Math.PI * Angle / 180000.0f;
        AngleSpeed += -0.4f / 3 * (float)Math.Cos(angleInRadians);
        Angle += AngleSpeed;
        angleInRadians = Math.PI * Angle / 180000.0f;

        var lengthX = Distance * Math.Cos(angleInRadians);
        var lengthY = -Distance * Math.Sin(angleInRadians);
        var finalPosition = (Vector2)SwingPosition + new Vector2((float)lengthX, (float)lengthY);
        var speed = finalPosition - (Owner.Position.Pixel + Owner.Position.Fraction);
        Owner.Speed.SetSpeed(speed);
    }
}