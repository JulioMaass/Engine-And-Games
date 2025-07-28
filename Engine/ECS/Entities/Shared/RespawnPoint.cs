using Engine.ECS.Entities.EntityCreation;

namespace Engine.ECS.Entities.Shared;

public class RespawnPoint : Entity
{
    public RespawnPoint()
    {
        EntityKind = EntityKind.StageEditing;
        // Basic components
        AddBasicComponents();
        SpawnManager.AutomaticSpawn = false;
        AddSprite("RespawnPoint", 32, 32, 16, 16);
    }

}
