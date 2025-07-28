using Engine.Managers;
using Engine.Managers.GameModes;
using Engine.Managers.StageEditing;
using Engine.Managers.StageHandling;
using MMDB.GameSpecific.Tilesets;

namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemStageOcean : Entity
{
    public MenuItemStageOcean()
    {
        AddStageMenuItemComponents();
        MenuItem.OnSelect = () =>
        {
            MenuManager.Clear();
            StageManager.CurrentStageName = "oceanStage";
            GameLoopManager.ResetGameLoop();
            StageEditor.TileMode.SetTilesetOfType(typeof(OceanTileset));
        };
    }
}