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
    public static float Curvature { get; set; }
    public static float Vignette { get; set; }
    public static float ScanlinesDepth { get; set; }
    public static float ScanlinesSpacing { get; set; }
    public static bool ScanRoll { get; set; }
    public static float VerticalLinesDepth { get; set; }

    public static void Update()
    {
        // NewPixie default settings
        WiggleToggle = true;
        Curvature = 2.0f;
        Vignette = 1.0f;
        ScanlinesDepth = 0.18f;
        ScanlinesSpacing = 1.5f;
        ScanRoll = true;
        VerticalLinesDepth = 0.23f;

        //// Space Miner settings
        //WiggleToggle = false;
        //Curvature = 0.1f;
        //Vignette = 0.75f;
        //ScanlinesDepth = 0.1f;
        //ScanlinesSpacing = 2.2f;
        //ScanRoll = false;
        //VerticalLinesDepth = 0.1f;

        // Preferences conversion
        var wiggleToggle = WiggleToggle ? 1.0f : 0.0f;
        var scanRoll = ScanRoll ? 1.0f : 0.0f;
        var curvature = Curvature;

        CrtEffect.Parameters["FrameCount"]?.SetValue(_frameCount++);
        CrtEffect.Parameters["OutputSize"]?.SetValue(OutputSize);

        CrtEffect.Parameters["WiggleToggle"]?.SetValue(wiggleToggle);
        CrtEffect.Parameters["Curvature"]?.SetValue(curvature);
        //CrtEffect.Parameters["Ghosting"]?.SetValue(0.5f);
        CrtEffect.Parameters["ScanlinesDepth"]?.SetValue(ScanlinesDepth);
        CrtEffect.Parameters["ScanlinesSpacing"]?.SetValue(ScanlinesSpacing);
        CrtEffect.Parameters["Vignette"]?.SetValue(Vignette);
        CrtEffect.Parameters["ScanRoll"]?.SetValue(scanRoll);
        CrtEffect.Parameters["VerticalLinesDepth"]?.SetValue(VerticalLinesDepth);

        if (_frameCount % 120 == 0)
            IsOn = !IsOn; // Toggle every 120 frames for testing purposes
    }
}
