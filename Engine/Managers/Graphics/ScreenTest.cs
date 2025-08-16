using Engine.GameSpecific;
using Engine.Helpers;
using Engine.Main;
using Engine.Types;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine.Managers.Graphics;
public static class ScreenTest
{
    public static int Index { get; set; }

    public class ConfiguredImage(Texture2D image, bool process)
    {
        public Texture2D Image { get; set; } = image;
        public bool Process { get; set; } = process;
    }

    public static List<ConfiguredImage> Images { get; } = new();

    public static void AddImage(Texture2D image, bool process)
    {
        Images.Add(new ConfiguredImage(image, process));
    }

    public static void Initialize()
    {
        if (GameManager.GameSpecificSettings.CurrentGame != GameId.CrtTest)
            return;

        AddImage(Drawer.TextureDictionary["Pixelated1x1"], false);
        AddImage(Drawer.TextureDictionary["Pixelated1x1"], true);
        AddImage(Drawer.TextureDictionary["Pixelated"], false);
        AddImage(Drawer.TextureDictionary["Pixelated"], true);
        AddImage(Drawer.TextureDictionary["NewPixie"], false);
        AddImage(Drawer.TextureDictionary["Mattias"], false);
        AddImage(Drawer.TextureDictionary["ColorTest"], false);
        AddImage(Drawer.TextureDictionary["ColorTest"], true);
        AddImage(Drawer.TextureDictionary["ColorTest NewPixie"], false);
        AddImage(Drawer.TextureDictionary["ColorTest Mattias"], false);
    }

    public static void Update()
    {
        if (GameManager.GameSpecificSettings.CurrentGame != GameId.CrtTest)
            return;

        if (Input.Right.Pressed)
        {
            Index++;
            if (Index >= Images.Count)
                Index = 0;
        }
        else if (Input.Left.Pressed)
        {
            Index--;
            if (Index < 0)
                Index = Images.Count - 1;
        }

        CrtManager.IsOn = Images[Index].Process;
    }

    public static void Draw()
    {
        if (GameManager.GameSpecificSettings.CurrentGame != GameId.CrtTest)
            return;

        Video.Graphics.GraphicsDevice.SetRenderTarget(Video.FinalRender);
        Video.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
        Video.SpriteBatch.Draw(Images[Index].Image, new IntRectangle(IntVector2.Zero, Settings.ScreenSize), CustomColor.White);
        Video.SpriteBatch.End();
    }
}
