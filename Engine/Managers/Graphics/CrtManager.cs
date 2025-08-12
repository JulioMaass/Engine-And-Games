using Engine.Main;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Managers.Graphics;

public static class CrtManager
{
    public static bool IsOn { get; set; } = true; // CRT effect toggle
    public static Effect CrtEffect;
    private static uint _frameCount;

    public static void Update()
    {
        CrtEffect.Parameters["OutputSize"].SetValue(Settings.ScreenScaledSize);
        CrtEffect.Parameters["FrameCount"].SetValue(_frameCount++);
        CrtEffect.Parameters["WiggleToggle"].SetValue(0.0f); // 1.0 to enable
        CrtEffect.Parameters["Curvature"].SetValue(1.0f);
        CrtEffect.Parameters["Ghosting"].SetValue(0.5f);
        CrtEffect.Parameters["Vignette"].SetValue(0.8f);
        CrtEffect.Parameters["ScanRoll"].SetValue(0.0f);

        //if (_frameCount % 120 == 0)
        //    IsOn = !IsOn; // Toggle every 120 frames for testing purposes
    }
}
