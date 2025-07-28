using Engine.Managers;
using Engine.Managers.GameModes;
using Engine.Managers.StageEditing;
using Engine.Managers.StageHandling;
using MMDB.GameSpecific.Tilesets;

namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemStageGas : Entity
{
    public MenuItemStageGas()
    {
        AddStageMenuItemComponents();
        MenuItem.OnSelect = () =>
        {
            MenuManager.Clear();
            StageManager.CurrentStageName = "gasStage";
            GameLoopManager.ResetGameLoop();
            StageEditor.TileMode.SetTilesetOfType(typeof(GasTileset));
        };
    }
}