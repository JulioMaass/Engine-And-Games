using Engine.Managers;
using Engine.Managers.GameModes;
using Engine.Managers.StageHandling;

namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemStageIntro : Entity
{
    public MenuItemStageIntro()
    {
        AddStageMenuItemComponents();
        MenuItem.OnSelect = () =>
        {
            MenuManager.Clear();
            StageManager.CurrentStageName = "defaultStagePlatformer";
            GameLoopManager.ResetGameLoop();
        };
    }
}