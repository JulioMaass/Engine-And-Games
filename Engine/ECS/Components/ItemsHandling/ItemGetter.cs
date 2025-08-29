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
        // Get Equipment
        if (entity.EquipmentItemStats?.Stats != null && Owner.EquipmentHolder != null)
        {
            if (entity.EquipmentItemStats.EquipKind != EquipKind.None)
                GlobalManager.Values.MainCharData.AddSwitchEquipment(entity.GetType(), 1, 1);
            else
                GlobalManager.Values.MainCharData.AddStackEquipment(entity.GetType());
        }

        // Get Resource
        if (entity.ResourceItemStatsList == null)
            return;
        foreach (var resourceItemStats in entity.ResourceItemStatsList)
        {
            if (resourceItemStats.ResourceType == ResourceType.Hp)
            {
                if (resourceItemStats.IncreaseKind is IncreaseKind.Max or IncreaseKind.CurrentAndMax)
                    Owner.DamageTaker.IncreaseMaxHp(resourceItemStats.Amount);
                if (resourceItemStats.IncreaseKind is IncreaseKind.Current or IncreaseKind.CurrentAndMax)
                    Owner.DamageTaker.HealHp(resourceItemStats.Amount);
            }
            else
            {
                if (resourceItemStats.IncreaseKind is IncreaseKind.Max or IncreaseKind.CurrentAndMax)
                    GlobalManager.Values.MainCharData.Resources.IncreaseMax(resourceItemStats.ResourceType,
                        resourceItemStats.Amount);
                if (resourceItemStats.IncreaseKind is IncreaseKind.Current or IncreaseKind.CurrentAndMax)
                    GlobalManager.Values.MainCharData.Resources.AddAmount(resourceItemStats.ResourceType,
                        resourceItemStats.Amount);
            }
        }
    }
}
