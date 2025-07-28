using Engine.Managers;
using Engine.Managers.GameModes;
using Engine.Managers.StageHandling;

namespace Mole.GameSpecific.Entities.MenuItems;

public class MenuItemStageIntro : Entity
{
    public MenuItemStageIntro()
    {
        AddPointedLabelMenuComponents();
        MenuItem.Label = "START";
        MenuItem.OnSelect = () =>
        {
            MenuManager.Clear();
            StageManager.CurrentStageName = "defaultStageTopdown";
            GameLoopManager.ResetGameLoop();
        };
    }
}