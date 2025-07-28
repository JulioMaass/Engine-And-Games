using Engine.Types;
using Microsoft.Xna.Framework;

namespace Engine.Managers.Graphics;

public static class ScreenTextManager
{
    private static string Text { get; set; }
    private static int TextDelay { get; set; }
    private static int TextDuration { get; set; }
    private static int Frame { get; set; }
    private static int OnDuration { get; set; }
    private static int OffDuration { get; set; }

    public static void CreateTemporaryText(string text, int textDuration, int textDelay, int onDuration = 1, int offDuration = 0)
    {
        Text = text;
        TextDuration = textDuration;
        TextDelay = textDelay;
        Frame = 0;
        OnDuration = onDuration;
        OffDuration = offDuration;
    }

    public static void Update()
    {
        Frame++;
        if (Frame >= TextDelay + TextDuration)
            Text = null;
    }

    public static void CancelText()
    {
        Text = null;
    }

    public static void DrawScreenText()
    {
        if (Text == null) return;
        if (Frame < TextDelay) return;
        if (OffDuration == 0) return;
        var lightFrame = (Frame - TextDelay) % (OnDuration + OffDuration);
        if ((Frame - TextDelay) % (OnDuration + OffDuration) < OnDuration)
            Video.SpriteBatch.DrawString(Drawer.MegaManFont, Text, IntVector2.New(173, 88), Color.White);
    }
}
