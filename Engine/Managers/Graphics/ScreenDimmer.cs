using System;

namespace Engine.Managers.Graphics;

public static class ScreenDimmer
{
    public static float Brightness { get; private set; } = 1f;
    private static float DimSpeed { get; set; }
    private static int DimDelay { get; set; }
    private static int Frame { get; set; }
    private static int DimDuration { get; set; }

    // TODO: ScreenDimmer doesn't stop dimming after the duration is over, it simply clamps the brightness to 0 or 1. If staying on other values is needed, this should be changed.
    // TODO: Check clashing of screen dimming and pause menu
    public static void DimScreen(float initialBrightness, float finalBrightness, int dimDuration, int dimDelay)
    {
        Brightness = initialBrightness;
        DimDuration = dimDuration;
        DimSpeed = (finalBrightness - initialBrightness) / dimDuration;
        DimDelay = dimDelay;
        Frame = 0;
    }

    public static void Update()
    {
        Frame++;
        if (Frame >= DimDelay + DimDuration)
            return;
        if (Frame >= DimDelay)
            Brightness += DimSpeed;
        Brightness = Math.Clamp(Brightness, 0f, 1f);
    }
}
