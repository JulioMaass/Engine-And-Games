namespace Engine.Managers.Graphics;

public static class ColorShaderManager
{
    public static void SetWhite()
    {
        Drawer.SpriteMasterShader.Parameters["ApplyWhite"].SetValue(true);
    }

    public static void SetGrayscale()
    {
        Drawer.SpriteMasterShader.Parameters["ApplyGrayscale"].SetValue(true);
    }

    public static void Reset()
    {
        Drawer.SpriteMasterShader.Parameters["ApplyWhite"].SetValue(false);
        Drawer.SpriteMasterShader.Parameters["ApplyGrayscale"].SetValue(false);
    }
}