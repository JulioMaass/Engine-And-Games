using Engine.Managers;
using SpaceMiner.GameSpecific.Entities.MenuLayouts;

namespace SpaceMiner.GameSpecific.Entities.MenuItems;

public class MenuItemWeaponTab : Entity
{
    public MenuItemWeaponTab()
    {
        AddPointedLabelMenuComponents();
        MenuItem.Label = "WEAPONS";
        MenuItem.OnSelect = () =>
        {
            MenuManager.CheckToSwapMenuArea(typeof(MenuAreaWeapons));
        };
    }
}