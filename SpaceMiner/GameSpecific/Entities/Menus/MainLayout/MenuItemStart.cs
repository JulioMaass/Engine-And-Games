using Engine.Managers.Graphics;
using SpaceMiner.GameSpecific.Entities.Menus.GameLayout;

namespace SpaceMiner.GameSpecific.Entities.Menus.MainLayout;

public class MenuItemStart : Entity
{
    public MenuItemStart()
    {
        AddPointedLabelMenuComponents(StringDrawer.CutePixelFont);
        MenuItem.Label = "START";
        MenuItem.ContainedMenuLayoutType = typeof(MenuLayoutGame);
    }
}