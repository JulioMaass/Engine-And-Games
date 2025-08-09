using Engine.ECS.Components.ItemsHandling;
using Engine.Managers.GlobalManagement;
using Engine.Types;

namespace Candle.GameSpecific;

public class CandleGlobalValues : Values
{
    protected override void CustomInitialize()
    {
        MainCharData.Resources.AddNew(ResourceType.Wax, 999, 0);

        MainCharData.AddEquipmentSlot(EquipKind.Weapon, SlotType.Switch);
        MainCharData.AddEquipmentSlot(EquipKind.Armor, SlotType.Switch);
        MainCharData.AddEquipmentSlot(EquipKind.Foot, SlotType.Switch);
    }

    protected override void CustomUpdate()
    {
    }
}
