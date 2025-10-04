using Engine.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Managers.Graphics;

public static class AccumulatorManager
{
    public static Effect AccumulateEffect;
    public static RenderTarget2D AccumulatorRender { get; } = new(Video.Graphics.GraphicsDevice, Settings.ScreenSize.X, Settings.ScreenSize.Y, false,
        SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

    public static void Run()
    {
        BlurManager.RenderBlur(4, 1.78f, 1f, AccumulatorRender, 1, 0);

        // Accumulate effect
        AccumulateEffect.Parameters["AccumulationStrength"].SetValue(0.65f);
        AccumulateEffect.Parameters["RenderCapValue"].SetValue(0.96f);
        AccumulateEffect.Parameters["AccumulatorTexture"].SetValue(AccumulatorRender);
        Video.Graphics.GraphicsDevice.SetRenderTarget(AccumulatorRender);
        Video.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, null, null, AccumulateEffect);
        Video.SpriteBatch.Draw(Video.FinalRender, Vector2.Zero, Color.White);
        Video.SpriteBatch.End();
        // Remove Accumulator render from index 1, so it uses register 0 for the next draw and is affected by SpriteBatch.Begin (SamplerState only affects the first texture)
        Video.Graphics.GraphicsDevice.Textures[1] = null;
    }
}
