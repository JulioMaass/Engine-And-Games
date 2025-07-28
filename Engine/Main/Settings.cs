using Engine.GameSpecific;
using Engine.Managers.StageEditing;
using Engine.Types;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.Main;

public static class Settings
{
    public static IntVector2 DisplaySize { get; } = IntVector2.New(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

    // Menu sizes
    public static int EditingMenuScale => 2;
    public static int EditingMenuWidth => GetEditingMenuWidth();
    public static IntVector2 EditingMenuSizeInTiles => StageEditor.TileMode.CurrentTileset.Texture.Bounds.Size / TileSize;
    public static IntVector2 EditingMenuPosition => IntVector2.New(ScreenScaledSize.Width, 0);

    // Room/Tile Sizes
    public static IntVector2 TileSize { get; set; }
    public static IntVector2 RoomSizeInTiles { get; set; }
    public static IntVector2 RoomSizeInPixels => RoomSizeInTiles * TileSize;

    // Screen sizes
    public static IntVector2 ScreenSize { get; set; }
    public static int ScreenScale { get; private set; }
    public static IntVector2 ScreenScaledSize => ScreenSize * ScreenScale;
    public static IntVector2 ZoomOutOffset { get; private set; }

    // Device specific settings
    public static bool LaptopModeIsOn { get; set; } = true; // Allows for panning without clicking

    public static void Initialize()
    {
        LaptopModeIsOn = DisplaySize.Width < 1920;
        SetScreenScale(LaptopModeIsOn ? 2 : 3);
    }

    public static int GetMaxScreenScale()
    {
        var xScale = DisplaySize.Width / ScreenSize.Width;
        var yScale = DisplaySize.Height / ScreenSize.Height;
        return Math.Min(xScale, yScale);
    }

    public static void SetScreenScale(int scale)
    {
        ScreenScale = scale;
        ZoomOutOffset = ScreenSize * (ScreenScale - 1) / 2;
    }

    private static int GetEditingMenuWidth()
    {
        var tileset = StageEditor.TileMode.CurrentTileset
            ?? StageEditor.TileMode.GetTilesetOfType(GameManager.GameSpecificSettings.DefaultTilesetType);
        return tileset.Texture.Width * EditingMenuScale;
    }
}
