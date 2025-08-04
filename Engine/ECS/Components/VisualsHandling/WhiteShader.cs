using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Engine.ECS.Components.VisualsHandling;

public class WhiteShader : Component
{
    private bool IsOn { get; set; }

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
