using Engine.Helpers;
using Engine.Main;
using Engine.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Managers.Graphics;

public static class StringDrawer
{
    // Fonts
    // Font characters:
    // !"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~
    // !"#$%&'()*+,-./
    //0123456789:;<=>?
    //@ABCDEFGHIJKLMNO
    //PQRSTUVWXYZ[\]^_
    //`abcdefghijklmno
    //pqrstuvwxyz{|}~
    public static SpriteFont PicoFont { get; private set; }                 // 3x5 Thin
    public static SpriteFont TinyUnicodeFont { get; private set; }          // 4x5 Thin
    public static SpriteFont Uni05XFont { get; private set; }               // 5x5 Thin
    public static SpriteFont PixelMasterFont { get; private set; }          // 5x7 Thin
    public static SpriteFont PressStart2PFont { get; private set; }         // 7x7 Bold
    public static SpriteFont CutePixelFont { get; private set; }            // 6x8 Thick
    public static SpriteFont PixellariFont { get; private set; }            // 8x11 Thick
    public static SpriteFont Pixel12X10Font { get; private set; }           // 10x12 Thick
    public static SpriteFont UpheavalFont { get; private set; }             // 11x10 Very Thick
    // Shadowed/Outlined Fonts
    public static SpriteFont HachicroFont { get; private set; }             // 5x5 Thin Outlined
    public static SpriteFont MegaManFont { get; private set; }              // 7x7 Bold Shadowed
    public static SpriteFont PressStart2PShadowFont { get; private set; }   // 7x7 Bold Shadowed
    // Edited fonts
    public static SpriteFont TinyUnicodeSoftFont { get; private set; }      // 4x5 Thin
    public static SpriteFont PressStartEditedFont { get; private set; }     // 7x7 Bold

    public static void LoadFonts()
    {
        PicoFont = GameManager.Game.Content.Load<SpriteFont>("Fonts/PicoFont");
        TinyUnicodeFont = GameManager.Game.Content.Load<SpriteFont>("Fonts/TinyUnicodeFont");
        TinyUnicodeFont.Spacing = 1;
        Uni05XFont = GameManager.Game.Content.Load<SpriteFont>("Fonts/Uni05xFont");
        Uni05XFont.Spacing = 1;
        PixelMasterFont = GameManager.Game.Content.Load<SpriteFont>("Fonts/PixelMasterFont");
        PixelMasterFont.Spacing = 1;
        PressStart2PFont = GameManager.Game.Content.Load<SpriteFont>("Fonts/PressStart2PFont");
        PressStart2PFont.Spacing = 1;
        CutePixelFont = GameManager.Game.Content.Load<SpriteFont>("Fonts/CutePixelFont");
        CutePixelFont.Spacing = 1;
        PixellariFont = GameManager.Game.Content.Load<SpriteFont>("Fonts/PixellariFont");
        Pixel12X10Font = GameManager.Game.Content.Load<SpriteFont>("Fonts/Pixel12X10Font");
        UpheavalFont = GameManager.Game.Content.Load<SpriteFont>("Fonts/UpheavalFont");
        UpheavalFont.Spacing = 1;
        HachicroFont = GameManager.Game.Content.Load<SpriteFont>("Fonts/HachicroFont");
        HachicroFont.Spacing = -1;
        MegaManFont = GameManager.Game.Content.Load<SpriteFont>("Fonts/MegaManFont");
        PressStart2PShadowFont = GameManager.Game.Content.Load<SpriteFont>("Fonts/PressStart2PShadowFont");
        TinyUnicodeSoftFont = GameManager.Game.Content.Load<SpriteFont>("Fonts/TinyUnicodeSoftFont");
        TinyUnicodeSoftFont.Spacing = 1;
        PressStartEditedFont = GameManager.Game.Content.Load<SpriteFont>("Fonts/PressStartEditedFont");
        PressStartEditedFont.Spacing = 1;
    }

    public static void DrawString(SpriteFont font, string @string, IntVector2 position, Color color) =>
        Video.SpriteBatch.DrawString(font, @string, position, color);

    public static void DrawStringOutlined(SpriteFont font, string @string, IntVector2 position, Color color, Color outlineColor = default)
    {
        outlineColor = outlineColor.DefaultTo(CustomColor.Black);
        for (var x = -1; x <= 1; x++)
            for (var y = -1; y <= 1; y++)
                if (x != 0 || y != 0)
                    Video.SpriteBatch.DrawString(font, @string, position + (x, y), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position, color);
    }

    public static void DrawStringOrthogonalOutlined(SpriteFont font, string @string, IntVector2 position, Color color, Color outlineColor = default)
    {
        outlineColor = outlineColor.DefaultTo(CustomColor.Black);
        Video.SpriteBatch.DrawString(font, @string, position + (-1, 0), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (1, 0), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (0, -1), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (0, 1), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position, color);
    }

    public static void DrawStringShadowed(SpriteFont font, string @string, IntVector2 position, Color color, Color outlineColor = default) // Down+Diagonal+Side shadow
    {
        outlineColor = outlineColor.DefaultTo(CustomColor.Black);
        Video.SpriteBatch.DrawString(font, @string, position + (1, 0), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (0, 1), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (1, 1), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position, color);
    }

    public static void DrawStringMegaShadowed(SpriteFont font, string @string, IntVector2 position, Color color, Color outlineColor = default) // Down+Diagonal shadow
    {
        outlineColor = outlineColor.DefaultTo(CustomColor.Black);
        Video.SpriteBatch.DrawString(font, @string, position + (0, 1), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (1, 1), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position, color);
    }

    public static void DrawStringOutlinedAndShadowed(SpriteFont font, string @string, IntVector2 position, Color color, bool squarish, Color outlineColor = default)
    {
        outlineColor = outlineColor.DefaultTo(CustomColor.Black);
        if (squarish)
            Video.SpriteBatch.DrawString(font, @string, position + (-1, -1), outlineColor); // Adds top left outline (works better for thicker fonts)
        Video.SpriteBatch.DrawString(font, @string, position + (-1, 0), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (-1, 1), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (0, -1), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (0, 1), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (0, 2), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (1, -1), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (1, 0), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (1, 1), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (1, 2), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (2, 0), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (2, 1), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position + (2, 2), outlineColor);
        Video.SpriteBatch.DrawString(font, @string, position, color);
    }
}
