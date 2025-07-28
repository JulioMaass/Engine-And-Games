using Engine.Managers;
using Engine.Managers.GameModes;
using Engine.Managers.StageEditing;
using Engine.Managers.StageHandling;
using MMDB.GameSpecific.Tilesets;

namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemStagePolice : Entity
{
    public MenuItemStagePolice()
    {
        AddStageMenuItemComponents();
        MenuItem.OnSelect = () =>
        {
            MenuManager.Clear();
            StageManager.CurrentStageName = "policeStage";
            GameLoopManager.ResetGameLoop();
            StageEditor.TileMode.SetTilesetOfType(typeof(PoliceTileset));
        };
    }
}