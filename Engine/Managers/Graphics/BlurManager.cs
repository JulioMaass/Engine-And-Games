using Engine.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Managers.Graphics;

public static class BlurManager
{
    public static Effect BlurEffect;
    public static RenderTarget2D PartialRender { get; } = Video.NewRenderTarget(Settings.ScreenSize);
    public static RenderTarget2D ScaledPartialRender { get; } = Video.NewRenderTarget(Settings.ScreenScaledSize);

    public static void RenderBlur(int width, float sigma, float bloomIntensity, RenderTarget2D source, bool scaled, RenderTarget2D destiny)
    {
        var partialRender = scaled ? ScaledPartialRender : PartialRender;
        var size = scaled ? Settings.ScreenScaledSize : Settings.ScreenSize;

        BlurEffect.Parameters["TargetSize"].SetValue(size);
        var distribution = GaussianDistribution(width, sigma);
        var sum = distribution.Sum();
        for (var i = 0; i < distribution.Count; i++)
        {
            distribution[i] /= sum;
            distribution[i] *= bloomIntensity;
        }
        var distributionArray = new float[21];
        var offset = 10 - width;
        for (var i = 0; i < distribution.Count; i++)
        {
            distributionArray[i + offset] = distribution[i];
        }

        // Horizontal pass
        BlurEffect.Parameters["Direction"].SetValue(new Vector2(1, 0)); // Horizontal
        BlurEffect.Parameters["Distribution"].SetValue(distributionArray);
        Video.Graphics.GraphicsDevice.SetRenderTarget(partialRender);
        Video.SpriteBatch.Begin(effect: BlurEffect);
        Video.SpriteBatch.Draw(source, Vector2.Zero, Color.White);
        Video.SpriteBatch.End();

        // Vertical pass
        BlurEffect.Parameters["Direction"].SetValue(new Vector2(0, 1)); // Vertical
        BlurEffect.Parameters["Distribution"].SetValue(distributionArray);
        Video.Graphics.GraphicsDevice.SetRenderTarget(destiny);
        Video.SpriteBatch.Begin(effect: BlurEffect);
        Video.SpriteBatch.Draw(partialRender, Vector2.Zero, Color.White);
        Video.SpriteBatch.End();
    }

    private static List<float> GaussianDistribution(int width, float sigma)
    {
        var distribution = new float[width + 1 + width];
        for (var i = -width; i <= width; i++)
            distribution[width + i] = (float)(Math.Exp(-(i * i) / (2 * sigma * sigma)) / (Math.PI * 2 * sigma * sigma));
        return distribution.ToList();
    }
}
