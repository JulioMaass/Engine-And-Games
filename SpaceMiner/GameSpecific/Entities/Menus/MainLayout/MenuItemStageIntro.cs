using Engine.Managers;
using Engine.Managers.GameModes;
using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;

namespace SpaceMiner.GameSpecific.Entities.Menus.MainLayout;

public class MenuItemStageIntro : Entity
{
    public MenuItemStageIntro()
    {
        AddPointedLabelMenuComponents(StringDrawer.CutePixelFont);
        MenuItem.Label = "START";
        MenuItem.OnSelect = () =>
        {
            MenuManager.Clear();
            StageManager.CurrentStageName = "defaultStageSpaceMiner";
            GameLoopManager.ResetGameLoop();
        };
    }
}