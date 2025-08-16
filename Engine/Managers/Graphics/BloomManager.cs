using Engine.ECS.Entities;
using Engine.Helpers;
using Engine.Main;
using Engine.Types;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Engine.Managers.Graphics;

public static class BloomManager
{
    public static bool IsOn { get; set; }
    public static RenderTarget2D BloomRender { get; } = Video.NewRenderTarget(Settings.ScreenSize);
    public static Effect BloomMaskEffect;
    public static bool DrawingBloom { get; private set; }

    public static void DrawBloomMask()
    {
        if (!IsOn)
            return;
        DrawingBloom = true;
        Video.Graphics.GraphicsDevice.SetRenderTarget(BloomRender);
        Video.Graphics.GraphicsDevice.Clear(CustomColor.Black);
        Video.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, BloomMaskEffect, Camera.Matrix);
        DrawBloomEntities();
        Video.SpriteBatch.End();
        DrawingBloom = false;
        BlurManager.RenderBlur(10, 5f, 2.65f, BloomRender, 1, 1);
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

    public static void DrawRender()
    {
        if (!IsOn)
            return;
        Video.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp);
        Video.SpriteBatch.Draw(BloomRender, new IntRectangle(IntVector2.Zero, Settings.ScreenSize), CustomColor.White);
        Video.SpriteBatch.End();
    }
}
