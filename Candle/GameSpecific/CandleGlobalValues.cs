using Engine.Managers.GlobalManagement;
using Engine.Types;

namespace Candle.GameSpecific;

public class CandleGlobalValues : Values
{
    protected override void CustomInitialize()
    {
        Resources.AddNew(ResourceType.Wax, 999, 0);
    }

    protected override void CustomUpdate()
    {
    }
}
