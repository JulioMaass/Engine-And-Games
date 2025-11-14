using Engine.Managers.Graphics;
using SpaceMiner.GameSpecific.Entities.Menus.StageSelectLayout;

namespace SpaceMiner.GameSpecific.Entities.Menus.GameLayout;

public class MenuItemStageSelect : Entity
{
    public MenuItemStageSelect()
    {
        AddPointedLabelMenuComponents(StringDrawer.CutePixelFont);
        MenuItem.Label = "STAGE SELECT";
        MenuItem.ContainedMenuLayoutType = typeof(MenuLayoutStageSelect);
    }
}