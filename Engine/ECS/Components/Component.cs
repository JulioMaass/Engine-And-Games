using Engine.ECS.Entities.EntityCreation;

namespace Engine.ECS.Components;

public class Component
{
    public Entity Owner { get; set; }
}