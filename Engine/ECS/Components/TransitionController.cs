using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using Microsoft.Xna.Framework;
using System;
using Engine.ECS.Components.ControlHandling.Conditions;

namespace Engine.ECS.Components;

public class TransitionController : Component
{
    public TransitionController(Entity owner)
    {
        Owner = owner;
    }

    public Condition UpCondition { get; set; }
    public Condition DownCondition { get; set; }
    public Condition SidesCondition { get; set; }
}
