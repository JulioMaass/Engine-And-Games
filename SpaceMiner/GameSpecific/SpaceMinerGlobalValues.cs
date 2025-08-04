using Engine.Managers.GlobalManagement;
using Engine.Types;

namespace SpaceMiner.GameSpecific;

public class SpaceMinerGlobalValues : Values
{
    protected override void CustomInitialize()
    {
        Resources.AddNew(ResourceType.OreGray, 999999, 0);
        Resources.AddNew(ResourceType.OreBlue, 999999, 0);
        Resources.AddNew(ResourceType.OreGreen, 999999, 0);
        Resources.AddNew(ResourceType.OreRed, 999999, 0);
        Resources.AddNew(ResourceType.OreYellow, 999999, 0);
        Resources.AddNew(ResourceType.OreOrange, 999999, 0);
        Resources.AddNew(ResourceType.OrePurple, 999999, 0);

        Resources.AddNew(ResourceType.MissileAtomic, 999999, 0);
        Resources.AddNew(ResourceType.MissileHoming, 999999, 0);

        //foreach (var resource in Resources.List)
        //    resource.Amount = 999999;
    }

    protected override void CustomUpdate()
    {
    }
}
