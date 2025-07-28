using Engine.ECS.Entities.EntityCreation;

namespace Engine.ECS.Components.ShootingHandling;

public class ShotProperties : Component
{
    public int ShotScreenPrice { get; set; }

    public ShotProperties(Entity owner)
    {
        Owner = owner;
    }
}