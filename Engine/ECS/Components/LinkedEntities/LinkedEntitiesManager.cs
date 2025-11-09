using Engine.ECS.Entities;
using Engine.ECS.Entities.EntityCreation;

namespace Engine.ECS.Components.LinkedEntities;

public class LinkedEntitiesManager : Component
{
    public Entity ShieldEntity { get; private set; }

    public LinkedEntitiesManager(Entity owner)
    {
        Owner = owner;
    }

    public void UpdatePositions()
    {
        if (ShieldEntity != null)
            ShieldEntity.Position.Pixel = Owner.Position.Pixel;
    }

    public void LinkShield(Entity shieldEntity)
    {
        ShieldEntity = shieldEntity;
        ShieldEntity.Position.Pixel = Owner.Position.Pixel;
    }

    public bool HasShield()
    {
        return ShieldEntity != null
               && EntityManager.GetAllEntities().Contains(ShieldEntity);
    }

    public void DeleteLinkedEntities()
    {
        EntityManager.DeleteEntity(ShieldEntity);
        ShieldEntity = null;
    }
}
