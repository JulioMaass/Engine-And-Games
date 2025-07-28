using Engine.ECS.Components.PhysicsHandling;
using Engine.ECS.Entities.EntityCreation;
using System;
using System.Collections.Generic;

namespace Engine.ECS.Components;

public class ComponentEnforcer : Component
{
    private bool ThrowError { get; set; } // Only throw errors if checking on a loop start, not when the entity is created (runs the enforcer on creation to add auto included components)

    public ComponentEnforcer(Entity owner)
    {
        Owner = owner;
    }

    public void FullCheck(bool throwError)
    {
        ThrowError = throwError;

        // Position components
        CheckForRequiredComponents(Owner.CollisionBox, new Dictionary<string, Component>
        {
            { "Position", Owner.Position },
        });
        CheckForRequiredComponents(Owner.Facing, new Dictionary<string, Component>
        {
            { "Position", Owner.Position },
            { "Sprite", Owner.Sprite },
        });
        // Physics components
        CheckForRequiredComponents(Owner.Speed, new Dictionary<string, Component>
        {
            { "Position", Owner.Position },
            { "Physics", Owner.Physics },
        });
        CheckForRequiredComponents(Owner.Gravity, new Dictionary<string, Component>
        {
            { "Speed", Owner.Speed },
        });
        CheckForRequiredComponents(Owner.SolidBehavior, new Dictionary<string, Component>
        {
            { "CollisionBox", Owner.CollisionBox },
        });
        // Visual components
        CheckForRequiredComponents(Owner.Sprite, new Dictionary<string, Component>
        {
            { "Position", Owner.Position },
            { "Facing", Owner.Facing },
        });
        CheckForRequiredComponents(Owner.Palette, new Dictionary<string, Component>
        {
            { "Sprite", Owner.Sprite },
        });
        // Combat components
        CheckForRequiredComponents(Owner.DamageDealer, new Dictionary<string, Component>
        {
            { "Alignment", Owner.Alignment },
        });
        CheckForRequiredComponents(Owner.DamageTaker, new Dictionary<string, Component>
        {
            { "Alignment", Owner.Alignment },
        });
        // Control components (player/AI)
        CheckForRequiredComponents(Owner.PlayerControl, new Dictionary<string, Component>
        {
            { "Facing", Owner.Facing },
        });
        // Physics system
        CheckForRequiredComponents(Owner.Physics, new Dictionary<string, Component> {
            { "Position", Owner.Position },
            { "Speed", Owner.Speed },
        });

        // Create states if needed
        if (Owner.Sprite != null)
            Owner.AddFillerStates();
    }

    private void CheckForRequiredComponents(Component component, Dictionary<string, Component> requiredComponents)
    {
        if (component == null) return;
        var missingComponents = new List<string>();

        foreach (var requiredComponent in requiredComponents)
        {
            if (requiredComponent.Value != null) continue;

            var autoIncludedComponent = CheckToAutoIncludeComponent(requiredComponent.Key);
            if (!autoIncludedComponent)
                missingComponents.Add(requiredComponent.Key);
        }

        if (ThrowError && missingComponents.Count > 0)
        {
            throw new Exception($"{component.Owner.GetType().Name}'s Component {component.GetType().Name} is missing required components: {string.Join(", ", missingComponents)}.");
        }
    }

    private bool CheckToAutoIncludeComponent(string componentName)
    {
        if (componentName == "Position")
        {
            Owner.AddPosition();
            return true;
        }
        if (componentName == "Facing")
        {
            Owner.AddFacing();
            return true;
        }
        if (componentName == "Speed")
        {
            Owner.AddSpeed();
            return true;
        }
        if (componentName == "Physics")
        {
            Owner.AddPhysics();
            return true;
        }
        if (componentName == "Gravity")
        {
            Owner.AddGravity(Gravity.DEFAULT_FORCE);
            return true;
        }
        return false;
    }
}