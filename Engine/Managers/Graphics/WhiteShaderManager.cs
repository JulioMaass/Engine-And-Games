using Microsoft.Xna.Framework.Graphics;

namespace Engine.Managers.Graphics;

public static class WhiteShaderManager
{
    public static void Set(bool isOn)
    {
        Drawer.WhiteShader.Parameters["IsOn"].SetValue(isOn);
    }

    public static void Reset()
    {
        Drawer.WhiteShader.Parameters["IsOn"].SetValue(false);
    }
}