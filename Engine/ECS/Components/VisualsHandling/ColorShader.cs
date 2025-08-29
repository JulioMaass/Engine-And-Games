using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.Graphics;

namespace Engine.ECS.Components.VisualsHandling;

public class ColorShader : Component
{
    public bool FlickerWhiteOn { get; set; }
    public bool GrayscaleOn { get; set; }


    public ColorShader(Entity owner)
    {
        Owner = owner;
    }

    public void Set()
    {
        SetWhite();
        SetGrayscale();
    }

    private void SetWhite()
    {
        if (!FlickerWhiteOn)
            return;
        if (Owner.DamageTaker?.IsInvincible() == true)
            ColorShaderManager.SetWhite();
    }

    private void SetGrayscale()
    {
        if (!GrayscaleOn)
            return;
        ColorShaderManager.SetGrayscale();
    }
}
