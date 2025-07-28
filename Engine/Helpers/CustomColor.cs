using Microsoft.Xna.Framework;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
#pragma warning disable CA2211

namespace Engine.Helpers;

public static class CustomColor
{
    // Transparent colors
    public static Color Transparent = new(0, 0, 0, 0);
    public static Color TransparentWhite = new(255, 255, 255, 127);
    public static Color TransparentRed = new(255, 0, 0, 127);
    public static Color TransparentGreen = new(0, 255, 0, 127);
    public static Color TransparentBlue = new(0, 0, 255, 127);
    public static Color TransparentYellow = new(255, 255, 0, 127);
    public static Color TransparentCyan = new(0, 255, 255, 127);
    public static Color TransparentMagenta = new(255, 0, 255, 127);
    public static Color TransparentGray = new(127, 127, 127, 127);

    // Solid colors
    public static Color White = new(255, 255, 255, 255);
    public static Color Black = new(0, 0, 0, 255);
    public static Color Red = new(255, 0, 0, 255);
    public static Color Gray = new(127, 127, 127, 255);
    public static Color DarkGray = new(63, 63, 63, 255);

    // Mega Man Colors
    public static Color MegaManWhite = new(248, 248, 248, 255);
    public static Color MegaManWeaponSelect = new(240, 184, 56, 255);

    // Candle Colors
    public static Color FlameRed = HexToColor("FFe34f00");
    public static Color FlameOrange = HexToColor("FFffa300");
    public static Color FlameYellow = HexToColor("FFffec27");

    // Pico 8 colors
    public static Color Pico0 = HexToColor("FF000000");
    public static Color Pico1 = HexToColor("FF1d2b53");
    public static Color Pico2 = HexToColor("FF7e2553");
    public static Color Pico3 = HexToColor("FF008751");
    public static Color Pico4 = HexToColor("FFab5236");
    public static Color Pico5 = HexToColor("FF5f574f");
    public static Color Pico6 = HexToColor("FFc2c3c7");
    public static Color Pico7 = HexToColor("FFfff1e8");
    public static Color Pico8 = HexToColor("FFff004d");
    public static Color Pico9 = HexToColor("FFffa300");
    public static Color Pico10 = HexToColor("FFffec27");
    public static Color Pico11 = HexToColor("FF00e436");
    public static Color Pico12 = HexToColor("FF29adff");
    public static Color Pico13 = HexToColor("FF83769c");
    public static Color Pico14 = HexToColor("FFff77a8");
    public static Color Pico15 = HexToColor("FFffccaa");
    public static Color Pico16 = HexToColor("FF291814");
    public static Color Pico17 = HexToColor("FF111d35");
    public static Color Pico18 = HexToColor("FF422136");
    public static Color Pico19 = HexToColor("FF125359");
    public static Color Pico20 = HexToColor("FF742f29");
    public static Color Pico21 = HexToColor("FF49333b");
    public static Color Pico22 = HexToColor("FFa28879");
    public static Color Pico23 = HexToColor("FFf3ef7d");
    public static Color Pico24 = HexToColor("FFbe1250");
    public static Color Pico25 = HexToColor("FFff6c24");
    public static Color Pico26 = HexToColor("FFa8e72e");
    public static Color Pico27 = HexToColor("FF00b543");
    public static Color Pico28 = HexToColor("FF065ab5");
    public static Color Pico29 = HexToColor("FF754665");
    public static Color Pico30 = HexToColor("FFff6e59");
    public static Color Pico31 = HexToColor("FFff9d81");

    public static Color PicoBlack = Pico0;
    public static Color PicoDarkBlue = Pico1;
    public static Color PicoDarkRed = Pico2;
    public static Color PicoDarkGreen = Pico3;
    public static Color PicoBrown = Pico4;
    public static Color PicoDarkGray = Pico5;
    public static Color PicoGray = Pico6;
    public static Color PicoWhite = Pico7;
    public static Color PicoRed = Pico8;
    public static Color PicoOrange = Pico9;
    public static Color PicoYellow = Pico10;
    public static Color PicoGreen = Pico11;
    public static Color PicoBlue = Pico12;
    public static Color PicoLavender = Pico13;
    public static Color PicoPink = Pico14;
    public static Color PicoPeach = Pico15;

    // Mixed colors
    public static Color PicoYellowOrange = MixedColor(PicoYellow, PicoOrange, 0.5f);
    public static Color PicoPeachPink = MixedColor(PicoPeach, PicoPink, 0.5f);

    private static Color HexToColor(string hexString)
    {
        // Convert string to hexadecimal integer
        var hexInt = int.Parse(hexString, System.Globalization.NumberStyles.HexNumber);

        // Convert hexadecimal integer to Color
        return new Color((hexInt >> 16) & 255, (hexInt >> 8) & 255, hexInt & 255, (hexInt >> 24) & 255);
    }

    public static Color MixedColor(Color color1, Color color2, float mix)
    {
        // Mix is the percentage of the first color
        var r = (byte)(color1.R * (1 - mix) + color2.R * mix);
        var g = (byte)(color1.G * (1 - mix) + color2.G * mix);
        var b = (byte)(color1.B * (1 - mix) + color2.B * mix);
        var a = (byte)(color1.A * (1 - mix) + color2.A * mix);
        return new Color(r, g, b, a);
    }
}
