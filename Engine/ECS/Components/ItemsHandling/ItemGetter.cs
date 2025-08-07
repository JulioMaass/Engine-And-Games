using Engine.ECS.Entities.EntityCreation;
using Engine.Managers.GlobalManagement;
using Engine.Types;

namespace Engine.ECS.Components.ItemsHandling;

public class ItemGetter : Component
{

    public ItemGetter(Entity owner)
    {
        Owner = owner;
    }

    public void GetItem(Entity entity)
    {
        // Get Resource
        if (entity.ResourceItemStats != null)
        {
            if (entity.ResourceItemStats.ResourceType == ResourceType.Hp)
                Owner.DamageTaker.HealHp(entity.ResourceItemStats.Amount);
            else
                GlobalManager.Values.Resources.AddAmount(entity.ResourceItemStats.ResourceType,
                    entity.ResourceItemStats.Amount);
        }

        // Get Equipment
        if (entity.EquipmentItemStats?.Stats != null && Owner.EquipmentHolder != null)
            GlobalManager.Values.AddEquipmentLevel(entity.GetType());
    }
}
