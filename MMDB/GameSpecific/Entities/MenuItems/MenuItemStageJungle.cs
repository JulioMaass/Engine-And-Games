using Engine.Managers;
using Engine.Managers.GameModes;
using Engine.Managers.StageEditing;
using Engine.Managers.StageHandling;
using MMDB.GameSpecific.Tilesets;

namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemStageJungle : Entity
{
    public MenuItemStageJungle()
    {
        AddStageMenuItemComponents();
        MenuItem.OnSelect = () =>
        {
            MenuManager.Clear();
            StageManager.CurrentStageName = "jungleStage";
            GameLoopManager.ResetGameLoop();
            StageEditor.TileMode.SetTilesetOfType(typeof(JungleTileset));
        };
    }
}