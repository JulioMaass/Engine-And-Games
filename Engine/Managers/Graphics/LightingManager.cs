using Engine.ECS.Entities;
using Engine.Main;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Engine.Managers.Graphics;

public static class LightingManager
{
    public static bool IsOn { get; set; }

    public static void ApplyLighting()
    {
        if (!IsOn)
            return;

        // Parameters
        var positions = new Vector2[16]; // Can have up to 16 light sources
        var radius1 = new float[16];
        var radius2 = new float[16];
        var colorDark = new Vector3(0.6f, 0.725f, 0.85f);
        var colorDim = new Vector3(0.85f, 0.9f, 0.95f);
        var colorBright = new Vector3(1.2f, 1.1f, 1.0f);
        var colors = new[] { colorDark, colorDim, colorBright };

        // Get all entities with a light source
        var id = 0;
        foreach (var entity in EntityManager.GetAllEntities().Where(entity => entity.LightSource != null))
        {
            positions[id] = new Vector2(entity.Position.Pixel.X, entity.Position.Pixel.Y) - (Vector2)Camera.Panning;
            (radius1[id], radius2[id]) = entity.LightSource.GetRadius();
            // Case zoomed out
            if (Camera.ZoomScale == (1, 1))
            {
                positions[id] /= 3f;
                positions[id] += (Vector2)Settings.RoomSizeInPixels / 3;
                radius1[id] /= 3f;
                radius2[id] /= 3f;
            }
            id++;
        }

        // Set shader parameters
        Drawer.AreaLightShader.Parameters["ScreenSize"].SetValue(Settings.ScreenSize);
        Drawer.AreaLightShader.Parameters["LightCount"].SetValue(id);
        Drawer.AreaLightShader.Parameters["LightPositions"].SetValue(positions);
        Drawer.AreaLightShader.Parameters["Radius1"].SetValue(radius1);
        Drawer.AreaLightShader.Parameters["Radius2"].SetValue(radius2);
        Drawer.AreaLightShader.Parameters["Colors"].SetValue(colors);

        // Draw shader
        Video.SpriteBatch.Begin(effect: Drawer.AreaLightShader);
        Video.SpriteBatch.Draw(Video.GameScreenRender, Vector2.Zero, Color.White);
        Video.SpriteBatch.End();
    }
}