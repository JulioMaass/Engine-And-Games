using Engine.Helpers;
using Engine.Main;
using Engine.Managers.GameModes;
using Engine.Managers.StageEditing;
using Engine.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Managers.Graphics;

public abstract class Video // Role: Draw game screen, HUD, and editing menu
{
    public static RenderTarget2D GameScreenRender { get; private set; } // Used to draw screen in the original aspect ratio, then later draw this render resized
    private static RenderTarget2D HudRender { get; set; }
    private static RenderTarget2D EditingMenuRender { get; set; }
    public static GraphicsDeviceManager Graphics { get; private set; }
    public static SpriteBatch SpriteBatch { get; private set; }

    public static void CreateGraphicsDevice()
    {
        Graphics = new GraphicsDeviceManager(GameManager.Game);
    }

    public static void Initialize()
    {
        SpriteBatch = new SpriteBatch(Graphics.GraphicsDevice);

        // Create surfaces for the original sized game screen, later to be resized
        GameScreenRender = NewRenderTarget(Settings.ScreenSize);
        HudRender = NewRenderTarget(Settings.ScreenSize);
        EditingMenuRender = NewRenderTarget(Settings.EditingMenuWidth, Settings.ScreenScaledSize.Height);


        ResizeScreen(Settings.ScreenScaledSize);
    }


    public static RenderTarget2D NewRenderTarget(int w, int h) =>
        new(Graphics.GraphicsDevice, w, h);
    public static RenderTarget2D NewRenderTarget(IntVector2 size) =>
        NewRenderTarget(size.Width, size.Height);

    public static void UpdateScreenRender(IntVector2 size)
    {
        GameScreenRender = NewRenderTarget(size);
    }

    public static void ResizeScreen(IntVector2 size)
    {
        Graphics.PreferredBackBufferWidth = size.Width;
        Graphics.PreferredBackBufferHeight = size.Height;
        Graphics.ApplyChanges();
    }

    public static void ToggleFullScreen()
    {
        Graphics.IsFullScreen = !Graphics.IsFullScreen;
        if (Graphics.IsFullScreen)
            ResizeScreen(Settings.DisplaySize);
        else
            ResizeScreen(Settings.ScreenScaledSize);
        Camera.UpdateFullscreenOffset();
    }

    public static void Draw()
    {
        DrawGameScreen();
        DrawHud();
        DrawEditingMenu();
        DrawToScreenScaled();
    }

    private static void DrawGameScreen()
    {
        Camera.MatrixUpdate();

        Graphics.GraphicsDevice.SetRenderTarget(GameScreenRender);
        Graphics.GraphicsDevice.Clear(CustomColor.DarkGray);
        SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, Drawer.WhiteShader, Camera.Matrix);
        GameLoopManager.Draw();
        SpriteBatch.End();

        BloomManager.DrawBloom();
        LightingManager.ApplyLighting();
    }

    private static void DrawHud()
    {
        Graphics.GraphicsDevice.SetRenderTarget(HudRender);
        Graphics.GraphicsDevice.Clear(CustomColor.Transparent);
        SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, Drawer.WhiteShader);
        Hud.Draw();
        SpriteBatch.End();
    }

    private static void DrawEditingMenu()
    {
        Graphics.GraphicsDevice.SetRenderTarget(EditingMenuRender);
        Graphics.GraphicsDevice.Clear(CustomColor.Black);
        SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
        StageEditor.DrawMenu();
        SpriteBatch.End();
    }

    private static void DrawToScreenScaled()
    {
        Graphics.GraphicsDevice.SetRenderTarget(null);
        SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
        SpriteBatch.Draw(GameScreenRender, new IntRectangle(Camera.FullScreenOffset, Settings.ScreenScaledSize), CustomColor.White);
        SpriteBatch.End();
        BloomManager.DrawRender();
        SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
        if (!StageEditor.IsOn)
            SpriteBatch.Draw(HudRender, new IntRectangle(Camera.FullScreenOffset, Settings.ScreenScaledSize), CustomColor.White);
        else
        {
            var menuX = Camera.FullScreenOffset.X + Settings.ScreenScaledSize.Width;
            SpriteBatch.Draw(EditingMenuRender, new Rectangle(menuX, Camera.FullScreenOffset.Y, Settings.EditingMenuWidth, Settings.ScreenScaledSize.Height), CustomColor.White);
        }
        // Apply screen dimmer
        if (ScreenDimmer.Brightness < 1f)
            Drawer.DrawRectangle(IntVector2.Zero, Settings.ScreenScaledSize, CustomColor.Black * (1f - ScreenDimmer.Brightness));
        SpriteBatch.End();
    }

    public static void DrawStringWithOutline(SpriteFont font, string text, Vector2 position, Color color)
    {
        for (var x = -1; x <= 1; x++)
            for (var y = -1; y <= 1; y++)
                if (x != 0 || y != 0)
                    SpriteBatch.DrawString(font, text, position + new Vector2(x, y), CustomColor.Black);
        SpriteBatch.DrawString(font, text, position, color);
    }
}