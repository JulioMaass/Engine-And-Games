using Engine.Managers.Graphics;
using MMDB.GameSpecific.Entities.MenuLayouts;

namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemLabelMain : Entity
{
    public MenuItemLabelMain()
    {
        AddPointedLabelMenuComponents(StringDrawer.MegaManFont);
        MenuItem.Label = "MAIN MENU";
        MenuItem.OnSelect = MenuItem.OpenContainedMenuLayout;
        MenuItem.ContainedMenuLayoutType = typeof(MenuLayoutMain);
    }
}