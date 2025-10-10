
using Engine.Managers.Graphics;

namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemLabelControls : Entity
{
    public MenuItemLabelControls()
    {
        AddPointedLabelMenuComponents(StringDrawer.MegaManFont);
        MenuItem.Label = "CONTROLS";
    }
}