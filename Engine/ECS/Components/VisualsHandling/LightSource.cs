using Engine.ECS.Entities.EntityCreation;
using Engine.Helpers;
using Engine.Types;
using System;

namespace Engine.ECS.Components.VisualsHandling;

public class LightSource : Component
{
    public IntVector2 RelativePosition { get; }
    public int Radius1 { get; private set; }
    public int Radius2 { get; private set; }
    private int OscillationOffset { get; } // Makes each light source oscillate at a different time

    public LightSource(Entity owner, IntVector2 relativePosition, int radius1, int radius2)
    {
        Owner = owner;
        RelativePosition = relativePosition;
        Radius1 = radius1;
        Radius2 = radius2;
        OscillationOffset = GetRandom.UnseededInt(100);
    }

    public void SetRadius(int radius1, int radius2)
    {
        Radius1 = radius1;
        Radius2 = radius2;
    }

    public (int radius1, int radius2) GetRadius()
    {
        var offsetFrame = (Owner.FrameHandler.CurrentFrame + OscillationOffset);
        var oscillation = (int)(Math.Sin(offsetFrame / 10.0f) * 2);
        var radius1 = Radius1 == 0 ? 0 : Radius1 + oscillation; // Only oscillate if radius is not 0
        var radius2 = Radius2 == 0 ? 0 : Radius2 + oscillation;
        return (radius1, radius2);
    }
}
