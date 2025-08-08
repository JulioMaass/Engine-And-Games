using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.GlobalManagement;

namespace Engine.ECS.Components.ItemsHandling;

public class EquipmentHolder : Component
{
    public CharData CharData => GlobalManager.Values.MainCharData;

    public EquipmentHolder(Entity owner)
    {
        Owner = owner;
    }
}
