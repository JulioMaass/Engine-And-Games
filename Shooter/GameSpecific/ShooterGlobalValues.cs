using Engine.Managers.GlobalManagement;
using Engine.Types;

namespace ShooterGame.GameSpecific;

public class ShooterGlobalValues : Values
{
    protected override void CustomInitialize()
    {
        Resources.AddNew(ResourceType.Score, 999999, 0);
    }

    protected override void CustomUpdate()
    {
    }
}
