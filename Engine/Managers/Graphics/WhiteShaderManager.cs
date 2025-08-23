namespace Engine.Managers.Graphics;

public static class WhiteShaderManager
{
    public static void Set(bool isOn)
    {
        Drawer.SpriteMasterShader.Parameters["ApplyWhite"].SetValue(isOn);
    }

    public static void Reset()
    {
        Drawer.SpriteMasterShader.Parameters["ApplyWhite"].SetValue(false);
    }
}