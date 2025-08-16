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
    private static RenderTarget2D PartialRender { get; } = Video.NewRenderTarget(Settings.ScreenSize);
    private static RenderTarget2D PartialRender2 { get; } = Video.NewRenderTarget(Settings.ScreenSize);

    public static void RenderBlur(int width, float sigma, float bloomIntensity, RenderTarget2D target, int horizontalPasses, int verticalPasses)
    {
        BlurEffect.Parameters["TargetSize"].SetValue(Settings.ScreenSize);
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
            distributionArray[i + offset] = distribution[i];

        // Set partial render
        Video.Graphics.GraphicsDevice.SetRenderTarget(PartialRender);
        Video.SpriteBatch.Begin(effect: null);
        Video.SpriteBatch.Draw(target, Vector2.Zero, Color.White);
        Video.SpriteBatch.End();

        // Horizontal passes
        var currentSource = PartialRender;
        var currentDest = PartialRender2;
        for (var pass = 0; pass < horizontalPasses; pass++)
        {
            BlurEffect.Parameters["Direction"].SetValue(new Vector2(1, 0));
            BlurEffect.Parameters["Distribution"].SetValue(distributionArray);
            Video.Graphics.GraphicsDevice.SetRenderTarget(currentDest);
            Video.SpriteBatch.Begin(effect: BlurEffect);
            Video.SpriteBatch.Draw(currentSource, Vector2.Zero, Color.White);
            Video.SpriteBatch.End();
            (currentSource, currentDest) = (currentDest, currentSource);
        }

        // Vertical pass
        for (var pass = 0; pass < verticalPasses; pass++)
        {
            BlurEffect.Parameters["Direction"].SetValue(new Vector2(0, 1));
            BlurEffect.Parameters["Distribution"].SetValue(distributionArray);
            Video.Graphics.GraphicsDevice.SetRenderTarget(currentDest);
            Video.SpriteBatch.Begin(effect: BlurEffect);
            Video.SpriteBatch.Draw(currentSource, Vector2.Zero, Color.White);
            Video.SpriteBatch.End();
            (currentSource, currentDest) = (currentDest, currentSource);
        }

        // Set destiny render
        Video.Graphics.GraphicsDevice.SetRenderTarget(target);
        Video.SpriteBatch.Begin(effect: null);
        Video.SpriteBatch.Draw(currentSource, Vector2.Zero, Color.White);
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