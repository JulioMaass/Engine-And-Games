using Engine.Main;
using Engine.Types;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Managers.Graphics;

public static class CrtManager
{
    public static bool IsOn { get; set; } = true; // CRT effect toggle
    public static Effect CrtEffect;
    private static uint _frameCount;
    public static IntVector2 OutputSize => Settings.ScreenScaledSize;

    // Preferences
    public static bool WiggleToggle { get; set; }
    public static float Curvature { get; set; } = 2.0f; // Mattias: 2.0f

    public static void Update()
    {
        // Preferences conversion
        var wiggleToggle = WiggleToggle ? 1.0f : 0.0f;
        var curvature = Curvature;

        CrtEffect.Parameters["FrameCount"].SetValue(_frameCount++);
        CrtEffect.Parameters["OutputSize"].SetValue(OutputSize);

        CrtEffect.Parameters["WiggleToggle"].SetValue(wiggleToggle);
        CrtEffect.Parameters["Curvature"].SetValue(curvature);
        CrtEffect.Parameters["Ghosting"].SetValue(0.5f);
        CrtEffect.Parameters["Vignette"].SetValue(1.0f);
        //CrtEffect.Parameters["ScanRoll"].SetValue(0.0f);

        //if (_frameCount % 120 == 0)
        //    IsOn = !IsOn; // Toggle every 120 frames for testing purposes
    }
}
