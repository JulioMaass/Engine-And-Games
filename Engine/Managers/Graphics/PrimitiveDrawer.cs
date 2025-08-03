using Engine.Main;
using Engine.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Engine.Managers.Graphics;

public static class PrimitiveDrawer
{
    private static GraphicsDevice GraphicsDevice => Video.Graphics.GraphicsDevice;
    private static BasicEffect Effect { get; set; }
    private static List<VertexPositionColor> TriangleListVertices { get; } = new();
    private static List<VertexPositionColor> LineListVertices { get; } = new();

    public static void Initialize()
    {
        Effect = new BasicEffect(GraphicsDevice);
        Effect.VertexColorEnabled = true;
        Effect.Projection = Matrix.CreateOrthographicOffCenter(
            0,
            Settings.ScreenSize.X,
            Settings.ScreenSize.Y,
            0,
            0,
            1
        );
    }

    public static void Reset()
    {
        Effect.CurrentTechnique.Passes[0].Apply();
        TriangleListVertices.Clear();
        LineListVertices.Clear();
    }

    public static void DrawThickLine(Vector2 start, Vector2 end, Color color, float thickness = 1f)
    {
        var dir = end - start;
        dir.Normalize();
        var perpendicular = new Vector2(-dir.Y, dir.X) * thickness / 2;

        var point1 = start + perpendicular;
        var point2 = start - perpendicular;
        var point3 = end + perpendicular;
        var point4 = end - perpendicular;
        DrawQuad(point1, point2, point3, point4, color);
    }

    public static void DrawRectangle(IntRectangle rectangle, Color color)
    {
        var topLeft = new Vector2(rectangle.Left, rectangle.Top);
        var topRight = new Vector2(rectangle.DrawingRight, rectangle.Top);
        var bottomLeft = new Vector2(rectangle.Left, rectangle.DrawingBottom);
        var bottomRight = new Vector2(rectangle.DrawingRight, rectangle.DrawingBottom);

        DrawQuad(topLeft, topRight, bottomLeft, bottomRight, color);
    }


    public static void DrawCircle(IntRectangle rectangle, Color color)
    {
        var center = rectangle.Position + (Vector2)rectangle.Size / 2f;
        var radiusX = rectangle.Width / 2f;
        var radiusY = rectangle.Height / 2f;
        var radius = Math.Max(radiusX, radiusY);
        var segments = Math.Ceiling(Math.PI / Math.Acos(1 - 0.1 / radius)); // 0.1 is the max deviation from a perfect circle 
        segments = (int)Math.Ceiling(segments / 4f) * 4; // Ensure segments are a multiple of 4 for symmetry
        var angleStep = MathHelper.TwoPi / segments;
        for (var i = 0; i < segments; i++)
        {
            var angle1 = angleStep * i;
            var angle2 = angleStep * (i + 1);
            var point1 = center + new Vector2((float)Math.Cos(angle1) * radiusX, (float)Math.Sin(angle1) * radiusY);
            var point2 = center + new Vector2((float)Math.Cos(angle2) * radiusX, (float)Math.Sin(angle2) * radiusY);
            DrawTriangle(center, point1, point2, color);
        }
    }

    public static void DrawCircleOutline(Vector2 center, float radius, Color color, int segments = 8)
    {
        var vertices = new VertexPositionColor[segments + 1];
        var angleStep = MathHelper.TwoPi / segments;
        for (var i = 0; i < segments; i++)
        {
            var angle1 = angleStep * i;
            var angle2 = angleStep * (i + 1);
            var point1 = new Vector2(center.X + radius * MathF.Cos(angle1), center.Y + radius * MathF.Sin(angle1));
            var point2 = new Vector2(center.X + radius * MathF.Cos(angle2), center.Y + radius * MathF.Sin(angle2));
            DrawLine(point1, point2, color);
        }
    }

    public static void DrawPerfectCircle(IntRectangle rectangle, Color color)
    {
        var center = rectangle.Position + (Vector2)rectangle.Size / 2f;
        var radiusX = rectangle.Width / 2f;
        var radiusY = rectangle.Height / 2f;
        var segments = (int)(radiusX * Math.PI * 2); // One segment per perimeter pixel
        for (var i = 0; i < segments; i++)
        {
            var angle1 = MathHelper.TwoPi * i / segments;
            var angle2 = MathHelper.TwoPi * (i + 1) / segments;
            var point1 = center + new Vector2((float)Math.Cos(angle1) * radiusX, (float)Math.Sin(angle1) * radiusY);
            var point2 = center + new Vector2((float)Math.Cos(angle2) * radiusX, (float)Math.Sin(angle2) * radiusY);
            DrawTriangle(center, point1, point2, color);
        }
    }

    public static void DrawQuad(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4, Color color)
    {
        DrawTriangle(point1, point2, point3, color);
        DrawTriangle(point3, point2, point4, color);
    }

    public static void DrawTriangle(Vector2 point1, Vector2 point2, Vector2 point3, Color color)
    {
        AddTriangleListVertex(point1, color);
        AddTriangleListVertex(point2, color);
        AddTriangleListVertex(point3, color);
    }

    private static void AddTriangleListVertex(Vector2 point, Color color)
    {
        var vertex = new VertexPositionColor(new Vector3(point, 0), color);
        TriangleListVertices.Add(vertex);
    }

    public static void DrawLine(Vector2 point1, Vector2 point2, Color color)
    {
        AddLineListVertex(point1, color);
        AddLineListVertex(point2, color);
    }

    private static void AddLineListVertex(Vector2 point, Color color)
    {
        var vertex = new VertexPositionColor(new Vector3(point, 0), color);
        LineListVertices.Add(vertex);
    }

    public static void Render()
    {
        if (TriangleListVertices.Count > 0)
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, TriangleListVertices.ToArray(), 0, TriangleListVertices.Count / 3);
        if (LineListVertices.Count > 0)
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, LineListVertices.ToArray(), 0, LineListVertices.Count / 2);
    }
}
