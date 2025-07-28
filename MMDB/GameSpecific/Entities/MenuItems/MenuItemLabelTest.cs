using MMDB.GameSpecific.Entities.MenuLayouts;

namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemLabelTest : Entity
{
    public MenuItemLabelTest()
    {
        AddPointedLabelMenuComponents();
        MenuItem.Label = "TEST";
        MenuItem.OnSelect = MenuItem.OpenContainedMenuLayout;
        MenuItem.ContainedMenuLayoutType = typeof(MenuLayoutTest);
    }
}