using Microsoft.Xna.Framework.Graphics;

namespace Engine.Managers.Graphics;

public static class PaletteManager
{
    private static Texture2D Texture { get; set; }
    private static int Index { get; set; }
    private static float FractionIndex => (Index + 0.5f) / Texture.Bounds.Height; // + 0.5 is to center the fraction and get the center of the pixel

    public static void SetPalette(Texture2D texture, int index)
    {
        Texture = texture;
        Index = index;

        Drawer.PaletteShader.Parameters["IsOn"].SetValue(true);
        Drawer.PaletteShader.Parameters["PaletteIndex"].SetValue(FractionIndex);
        Drawer.PaletteShader.Parameters["PaletteTexture"].SetValue(Texture);
    }

    public static void ResetPalette()
    {
        Drawer.PaletteShader.Parameters["IsOn"].SetValue(false);
    }
}