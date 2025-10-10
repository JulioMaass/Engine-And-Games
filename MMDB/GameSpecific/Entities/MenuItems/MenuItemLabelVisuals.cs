
using Engine.Managers.Graphics;

namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemLabelVisuals : Entity
{
    public MenuItemLabelVisuals()
    {
        AddPointedLabelMenuComponents(StringDrawer.MegaManFont);
        MenuItem.Label = "VISUALS";
    }
}