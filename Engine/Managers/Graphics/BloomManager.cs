using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Main;
using Engine.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Engine.Managers.Graphics;

public static class BloomManager
{
    public static bool IsOn { get; set; }
    public static RenderTarget2D GameScreenBloomRender { get; } = Video.NewRenderTarget(Settings.ScreenSize);
    public static RenderTarget2D GameScreenBloomBlurredXRender { get; } = Video.NewRenderTarget(Settings.ScreenSize);
    public static RenderTarget2D GameScreenBloomBlurredYRender { get; } = Video.NewRenderTarget(Settings.ScreenSize);
    public static Effect BloomEffect;
    public static Effect BloomMaskEffect;
    public static bool DrawingBloom { get; private set; }

    public static void DrawBloom()
    {
        if (!IsOn)
            return;
        DrawingBloom = true;
        Video.Graphics.GraphicsDevice.SetRenderTarget(GameScreenBloomRender);
        Video.Graphics.GraphicsDevice.Clear(CustomColor.Black);
        Video.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, BloomMaskEffect, Camera.Matrix);
        DrawBloomEntities();
        Video.SpriteBatch.End();
        DrawingBloom = false;
        GenerateBloomRender();
    }

    private static void DrawBloomEntities()
    {
        EntityManager.GetAllEntities()
            .OrderBy(entity => entity.LayerId)
            .ThenBy(entity => entity.EntityKind)
            .ThenBy(entity => entity.DrawOrder)
            .ToList()
            .ForEach(entity => entity.Draw());
    }

    private static void GenerateBloomRender()
    {
        BloomEffect.Parameters["TargetWidth"].SetValue(Settings.ScreenSize.X);
        BloomEffect.Parameters["TargetHeight"].SetValue(Settings.ScreenSize.Y);

        // Horizontal pass
        BloomEffect.Parameters["Direction"].SetValue(new Vector2(1, 0)); // Horizontal
        Video.Graphics.GraphicsDevice.SetRenderTarget(GameScreenBloomBlurredXRender);
        Video.SpriteBatch.Begin(effect: BloomEffect);
        Video.SpriteBatch.Draw(GameScreenBloomRender, Vector2.Zero, Color.White);
        Video.SpriteBatch.End();

        // Vertical pass
        BloomEffect.Parameters["Direction"].SetValue(new Vector2(0, 1)); // Vertical
        Video.Graphics.GraphicsDevice.SetRenderTarget(GameScreenBloomBlurredYRender);
        Video.SpriteBatch.Begin(effect: BloomEffect);
        Video.SpriteBatch.Draw(GameScreenBloomBlurredXRender, Vector2.Zero, Color.White);
        Video.SpriteBatch.End();
    }

    public static void DrawRender()
    {
        if (!IsOn)
            return;
        Video.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp);
        Video.SpriteBatch.Draw(GameScreenBloomBlurredYRender, new IntRectangle(Camera.FullScreenOffset, Settings.ScreenScaledSize), CustomColor.White);
        Video.SpriteBatch.End();
    }
}
