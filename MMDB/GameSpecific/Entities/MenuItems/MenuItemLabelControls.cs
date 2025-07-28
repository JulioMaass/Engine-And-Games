
namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemLabelControls : Entity
{
    public MenuItemLabelControls()
    {
        AddPointedLabelMenuComponents();
        MenuItem.Label = "CONTROLS";
    }
}