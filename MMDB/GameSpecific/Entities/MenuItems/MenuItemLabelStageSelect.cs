using Engine.Managers.Graphics;
using MMDB.GameSpecific.Entities.MenuLayouts;

namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemLabelStageSelect : Entity
{
    public MenuItemLabelStageSelect()
    {
        AddPointedLabelMenuComponents(StringDrawer.MegaManFont);
        MenuItem.Label = "STAGE SELECT";
        MenuItem.OnSelect = MenuItem.OpenContainedMenuLayout;
        MenuItem.ContainedMenuLayoutType = typeof(MenuLayoutStageSelect);
    }
}