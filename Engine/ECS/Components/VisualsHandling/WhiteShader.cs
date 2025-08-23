using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.Graphics;

namespace Engine.ECS.Components.VisualsHandling;

public class WhiteShader : Component
{
    public WhiteShader(Entity owner)
    {
        Owner = owner;
    }

    public void Set()
    {
        if (Owner.DamageTaker?.IsInvincible() == true)
            WhiteShaderManager.Set(true);
    }
}
