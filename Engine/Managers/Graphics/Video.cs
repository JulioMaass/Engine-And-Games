using Engine.Helpers;
using Engine.Main;
using Engine.Managers.GameModes;
using Engine.Managers.StageEditing;
using Engine.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Engine.Managers.Graphics;

public abstract class Video // Role: Draw game screen, HUD, and editing menu
{
    public static RenderTarget2D GameScreenRender { get; private set; } // Used to draw screen in the original aspect ratio, then later draw this render resized
    private static RenderTarget2D HudRender { get; set; }
    private static RenderTarget2D EditingMenuRender { get; set; }
    public static RenderTarget2D FinalRender { get; private set; }
    private static RenderTarget2D PostFxRender { get; set; }
    public static GraphicsDeviceManager Graphics { get; private set; }
    public static SpriteBatch SpriteBatch { get; private set; }
    private static bool CameraOn { get; set; } // Tells if the camera matrix should be applied
    private static Matrix? Matrix => CameraOn ? Camera.Matrix : null;
    private static SpriteSortMode CurrentSpriteSortMode { get; set; } = SpriteSortMode.Deferred;
    private static int SpriteSortModeSwitchCount { get; set; }

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
        FinalRender = NewRenderTarget(Settings.ScreenSize);
        EditingMenuRender = NewRenderTarget(Settings.EditingMenuWidth, Settings.ScreenScaledSize.Height);
        PostFxRender = NewRenderTarget(Settings.ScreenScaledSize);

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
        ResetSpriteSortModeSwitchCount();
        DrawGameScreen();
        DrawHud();
        DrawEditingMenu();
        DrawFinalRender();
        ScreenTest.Draw();
        DrawPostFx();
    }

    private static void ResetSpriteSortModeSwitchCount()
    {
#if DEBUG
        if (SpriteSortModeSwitchCount > 20)
            Debugger.Break(); // Too many sprite sort mode switches in one frame, try to optimize
#endif
        SpriteSortModeSwitchCount = 0;
    }

    private static void DrawGameScreen()
    {
        Camera.MatrixUpdate();

        CameraOn = true;
        Graphics.GraphicsDevice.SetRenderTarget(GameScreenRender);
        Graphics.GraphicsDevice.Clear(CustomColor.DarkGray);
        CurrentSpriteSortMode = SpriteSortMode.Deferred;
        SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, Drawer.SpriteMasterShader, Matrix);
        GameLoopManager.Draw();
        SpriteBatch.End();
        CameraOn = false;

        BloomManager.DrawBloomMask();
        LightingManager.ApplyLighting();
    }

    private static void DrawHud()
    {
        Graphics.GraphicsDevice.SetRenderTarget(HudRender);
        Graphics.GraphicsDevice.Clear(CustomColor.Transparent);
        CurrentSpriteSortMode = SpriteSortMode.Deferred;
        SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, Drawer.SpriteMasterShader, Matrix);
        Hud.Draw();
        SpriteBatch.End();
    }

    public static void SwitchSpriteSortMode(SpriteSortMode spriteSortMode) // Switch to immediate when using shaders with draw call specific parameters
    {
        if (CurrentSpriteSortMode == spriteSortMode)
            return;
        CurrentSpriteSortMode = spriteSortMode;
        SpriteSortModeSwitchCount++;
        SpriteBatch.End();
        SpriteBatch.Begin(spriteSortMode, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, Drawer.SpriteMasterShader, Matrix);
    }

    private static void DrawEditingMenu()
    {
        Graphics.GraphicsDevice.SetRenderTarget(EditingMenuRender);
        Graphics.GraphicsDevice.Clear(CustomColor.Black);
        CurrentSpriteSortMode = SpriteSortMode.Deferred;
        SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
        StageEditor.DrawMenu();
        SpriteBatch.End();
    }

    private static void DrawFinalRender()
    {
        Graphics.GraphicsDevice.SetRenderTarget(FinalRender);
        SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
        SpriteBatch.Draw(GameScreenRender, new IntRectangle(IntVector2.Zero, Settings.ScreenSize), CustomColor.White);
        SpriteBatch.End();
        BloomManager.DrawRender();
        SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
        if (!StageEditor.IsOn)
            SpriteBatch.Draw(HudRender, new IntRectangle(IntVector2.Zero, Settings.ScreenSize), CustomColor.White);
        // Apply screen dimmer
        if (ScreenDimmer.Brightness < 1f)
            Drawer.DrawRectangle(IntVector2.Zero, Settings.ScreenSize, CustomColor.Black * (1f - ScreenDimmer.Brightness));
        SpriteBatch.End();
    }

    private static void DrawPostFx()
    {
        // Apply blurs
        if (CrtManager.IsOn)
            AccumulatorManager.Run();
        Graphics.GraphicsDevice.SetRenderTarget(PostFxRender);
        var render = CrtManager.IsOn ? AccumulatorManager.AccumulatorRender : FinalRender;
        var sampler = CrtManager.IsOn ? SamplerState.LinearClamp : SamplerState.PointClamp;
        SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, sampler);
        SpriteBatch.Draw(render, new IntRectangle(Camera.FullScreenOffset, Settings.ScreenScaledSize), Color.White);
        SpriteBatch.End();

        // Apply crt shader
        Graphics.GraphicsDevice.SetRenderTarget(null);
        var effect = CrtManager.IsOn ? CrtManager.CrtEffect : null;
        SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, null, null, effect);
        SpriteBatch.Draw(PostFxRender, new IntRectangle(Camera.FullScreenOffset, Settings.ScreenScaledSize), Color.White);
        if (StageEditor.IsOn)
            SpriteBatch.Draw(EditingMenuRender, new Rectangle(Settings.ScreenScaledSize.Width, 0, Settings.EditingMenuWidth, Settings.ScreenScaledSize.Height), CustomColor.White);
        SpriteBatch.End();
    }
}