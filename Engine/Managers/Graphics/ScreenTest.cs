using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Managers.Graphics;
public static class ScreenTest
{
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
        AddImage(Drawer.TextureDictionary["Pixelated"], false);
        AddImage(Drawer.TextureDictionary["Pixelated"], true);
        AddImage(Drawer.TextureDictionary["NewPixie"], false);
        AddImage(Drawer.TextureDictionary["Mattias"], false);
        AddImage(Drawer.TextureDictionary["ColorTest"], false);
        AddImage(Drawer.TextureDictionary["ColorTest"], true);
        AddImage(Drawer.TextureDictionary["ColorTestNewPixie"], false);
        AddImage(Drawer.TextureDictionary["ColorTestMattias"], false);
    }
}
