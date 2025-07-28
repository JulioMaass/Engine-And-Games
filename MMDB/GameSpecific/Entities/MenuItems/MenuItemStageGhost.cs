using Engine.Managers;
using Engine.Managers.GameModes;
using Engine.Managers.StageEditing;
using Engine.Managers.StageHandling;
using MMDB.GameSpecific.Tilesets;

namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemStageGhost : Entity
{
    public MenuItemStageGhost()
    {
        AddStageMenuItemComponents();
        MenuItem.OnSelect = () =>
        {
            MenuManager.Clear();
            StageManager.CurrentStageName = "ghostStage";
            GameLoopManager.ResetGameLoop();
            StageEditor.TileMode.SetTilesetOfType(typeof(GhostTileset));
        };
    }
}