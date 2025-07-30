using Engine.Helpers;
using Engine.Main;
using Engine.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace Engine.Managers.Graphics;

public static class Drawer
{
    private static Texture2D WhitePixel { get; set; } // a single white pixel
    public static Dictionary<string, Texture2D> TextureDictionary { get; } = new();
    public static SpriteFont PicoFont { get; private set; }
    public static SpriteFont MegaManFont { get; private set; }
    public static SpriteFont HpFont { get; private set; }
    public static SpriteFont HpFontMap { get; private set; }

    // Shaders
    public static Effect PaletteShader { get; private set; }
    public static Effect AreaLightShader { get; private set; }

    // Default values for drawing
    private static Color DefaultColor { get; set; } = CustomColor.White;
    private static float DefaultRotation { get; set; } = 0.0f;
    private static Vector2 DefaultRotationOrigin { get; set; } = new(0, 0);
    private static float DefaultDepth { get; set; } = 0.0f;
    public static Color BackgroundColor { get; set; } = CustomColor.Black;

    public static void Initialize()
    {
        // While pixel to draw rectangles, etc
        WhitePixel = new Texture2D(Video.Graphics.GraphicsDevice, 1, 1);
        WhitePixel.SetData(new[] { CustomColor.White });
    }

    public static void LoadContent()
    {
        // Textures
        LoadTextures("EngineTextures");
        LoadTextures("Textures");

        // Fonts
        PicoFont = GameManager.Game.Content.Load<SpriteFont>("Fonts/PicoFont");
        MegaManFont = GameManager.Game.Content.Load<SpriteFont>("Fonts/MegaManFont");
        HpFont = GameManager.Game.Content.Load<SpriteFont>("Fonts/HpFont");
        HpFont.Spacing = -1;
        HpFontMap = GameManager.Game.Content.Load<SpriteFont>("Fonts/HpFontMap");
        HpFontMap.Spacing = 1;

        // Shaders
        PaletteShader = GameManager.Game.Content.Load<Effect>("SpriteShaders/PaletteShader");
        AreaLightShader = GameManager.Game.Content.Load<Effect>("SpriteShaders/AreaLightShader");
        BloomManager.BloomEffect = GameManager.Game.Content.Load<Effect>("SpriteShaders/BloomShader");
        BloomManager.BloomMaskEffect = GameManager.Game.Content.Load<Effect>("SpriteShaders/BloomMaskShader");
    }

    private static void LoadTextures(string folder)
    {
        var folderPath = "Content/" + folder;
        var fileNames = Directory.GetFiles(folderPath, "*.xnb", SearchOption.AllDirectories);
        foreach (var fileName in fileNames)
        {
            // Get the texture and name
            var textureName = Path.GetFileNameWithoutExtension(fileName);
            var texturePath = fileName.Substring(fileName.IndexOf(folder));
            texturePath = texturePath.Replace("\\", "/").Replace(".xnb", "");
            var texture = GameManager.Game.Content.Load<Texture2D>(texturePath);
            // Add the texture name and texture to the dictionary
            TextureDictionary.Add(textureName, texture);
        }
    }

    public static void DrawRectangle(IntVector2 position, IntVector2 size, Color color)
    {
        Video.SpriteBatch.Draw(WhitePixel, new IntRectangle(position, size), color);
    }

    public static void DrawCircle(IntVector2 position, int radius)
    {
        Video.SpriteBatch.Draw(
            TextureDictionary.GetValueOrDefault("MegaCircle"),
            new IntRectangle(position - radius / 2, radius, radius),
            new IntRectangle(0, 0, 1000, 1000),
            DefaultColor,
            DefaultRotation,
            DefaultRotationOrigin,
            SpriteEffects.None,
            DefaultDepth);
    }

    public static void DrawRectangleOutline(IntVector2 position, IntVector2 size, Color color, int thickness = 1)
    {
        DrawRectangleOutline(position, size, color, thickness, thickness);
    }

    public static void DrawRectangleOutline(IntVector2 position, IntVector2 size, Color color, int xThickness, int yThickness)
    {
        var horizontalSize = IntVector2.New(size.Width, yThickness);
        var verticalSize = IntVector2.New(xThickness, size.Height - yThickness * 2);

        // Top border
        DrawRectangle(position, horizontalSize, color);
        //Bottom border
        var bottomPosition = position + (0, size.Height - yThickness);
        DrawRectangle(bottomPosition, horizontalSize, color);
        // Left border
        var leftPosition = position + (0, yThickness);
        DrawRectangle(leftPosition, verticalSize, color);
        // Right border
        var rightPosition = position + (size.Width - xThickness, yThickness);
        DrawRectangle(rightPosition, verticalSize, color);
    }

    public static void DrawTextureRectangleAt(Texture2D texture, IntRectangle sourceRectangle, IntVector2 position, bool flipped = false, Color color = default)
    {
        var effects = SpriteEffects.None;
        if (flipped) effects = SpriteEffects.FlipHorizontally;
        if (color == default) color = DefaultColor;
        Video.SpriteBatch.Draw(texture, position, sourceRectangle, color, DefaultRotation, DefaultRotationOrigin, 1, effects, DefaultDepth);
    }

    public static void DrawTextureRectangleAt(Texture2D texture, IntRectangle sourceRectangle, IntVector2 position, IntVector2 stretchedSize, Color color, bool flipped = false)
    {
        var effects = SpriteEffects.None;
        if (flipped) effects = SpriteEffects.FlipHorizontally;

        if (stretchedSize == IntVector2.Zero)
            stretchedSize = sourceRectangle.Size;
        var destinationRectangle = new IntRectangle(position, stretchedSize);

        Video.SpriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, DefaultRotation, DefaultRotationOrigin, effects, DefaultDepth);
    }

    public static void DrawFullTextureAt(Texture2D texture, IntVector2 position)
    {
        var sourceRectangle = new IntRectangle(IntVector2.Zero, texture.GetSize());
        DrawTextureRectangleAt(texture, sourceRectangle, position);
    }

    public static IntRectangle GetSourceRectangleFromId(Texture2D texture, IntVector2 spriteSheetOrigin, IntVector2 size, int id)
    {
        var sheetColumns = texture.Width / size.Width;

        // Calculate the 1st sprite position in the sheet
        var spriteSheetOriginColumn = spriteSheetOrigin.X / size.Width;
        var spriteSheetOriginRow = spriteSheetOrigin.Y / size.Height;

        // Take sheet origin into account to calculate the sprite position
        var offsetId = id + spriteSheetOriginColumn + spriteSheetOriginRow * sheetColumns;

        // Calculate the sprite position in the sheet
        var spriteColumn = offsetId % sheetColumns;
        var spriteRow = offsetId / sheetColumns;
        var spriteX = spriteColumn * size.Width;
        var spriteY = spriteRow * size.Height;
        return new IntRectangle(spriteX, spriteY, size);
    }

    public static void DrawEmptyBackground(IntVector2 positionInTiles, IntVector2 sizeInTiles, int scale = 1)
    {
        var texture = TextureDictionary.GetValueOrDefault("EmptyBackground");
        for (var x = 0; x < sizeInTiles.Width; x++)
        {
            for (var y = 0; y < sizeInTiles.Height; y++)
            {
                var pixelPosition = (IntVector2.New(x, y) + positionInTiles) * Settings.TileSize * scale;
                DrawTextureRectangleAt(texture, texture.Bounds, pixelPosition, Settings.TileSize * scale, CustomColor.White);
            }
        }
    }
}
