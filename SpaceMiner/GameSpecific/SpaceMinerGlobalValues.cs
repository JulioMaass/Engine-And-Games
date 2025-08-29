using Engine.ECS.Components.ItemsHandling;
using Engine.Managers.GlobalManagement;
using Engine.Types;

namespace SpaceMiner.GameSpecific;

public class SpaceMinerGlobalValues : Values
{
    protected override void CustomInitialize()
    {
        MainCharData.Resources.AddNew(ResourceType.OreGray, 999999, 0);
        MainCharData.Resources.AddNew(ResourceType.OreBlue, 999999, 0);
        MainCharData.Resources.AddNew(ResourceType.OreGreen, 999999, 0);
        MainCharData.Resources.AddNew(ResourceType.OreRed, 999999, 0);
        MainCharData.Resources.AddNew(ResourceType.OreYellow, 999999, 0);
        MainCharData.Resources.AddNew(ResourceType.OreOrange, 999999, 0);
        MainCharData.Resources.AddNew(ResourceType.OrePurple, 999999, 0);

        MainCharData.Resources.AddNew(ResourceType.MissileAtomic, 10, 0);
        MainCharData.Resources.AddNew(ResourceType.MissileHoming, 10, 0);
        MainCharData.Resources.AddNew(ResourceType.MissileSpray, 10, 0);
        MainCharData.Resources.AddNew(ResourceType.MissileDrill, 10, 0);

        //foreach (var resource in MainCharData.Resources.List)
        //    resource.Add(999999);

        MainCharData.AddEquipmentSlot(EquipKind.None, SlotType.Stack);
        MainCharData.AddEquipmentSlot(EquipKind.Weapon, SlotType.Switch);
        MainCharData.AddEquipmentSlot(EquipKind.SecondaryWeapon, SlotType.Switch);
    }

    protected override void CustomUpdate()
    {
    }
}
