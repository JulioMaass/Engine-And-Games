using Engine.Managers;
using Engine.Managers.GameModes;
using Engine.Managers.Graphics;
using Engine.Managers.StageHandling;

namespace SpaceMiner.GameSpecific.Entities.Menus.GameLayout;

public class MenuItemPermanentUpgrades : Entity
{
    public MenuItemPermanentUpgrades()
    {
        AddPointedLabelMenuComponents(StringDrawer.CutePixelFont);
        MenuItem.Label = "PERMANENT UPGRADES";
        MenuItem.OnSelect = () =>
        {
            MenuManager.Clear();
            StageManager.CurrentStageName = "defaultStageSpaceMiner";
            GameLoopManager.ResetGameLoop();
        };
    }
}