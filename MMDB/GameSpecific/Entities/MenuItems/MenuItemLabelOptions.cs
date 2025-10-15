using Engine.Managers.Graphics;
using MMDB.GameSpecific.Entities.MenuLayouts;

namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemLabelOptions : Entity
{
    public MenuItemLabelOptions()
    {
        AddPointedLabelMenuComponents(StringDrawer.MegaManFont);
        MenuItem.Label = "OPTIONS";
        MenuItem.ContainedMenuLayoutType = typeof(MenuLayoutOptions);
    }
}