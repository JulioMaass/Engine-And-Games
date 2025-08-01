using Engine.Managers;
using SpaceMiner.GameSpecific.Entities.MenuLayouts;

namespace SpaceMiner.GameSpecific.Entities.MenuItems;

public class MenuItemMissileTab : Entity
{
    public MenuItemMissileTab()
    {
        AddPointedLabelMenuComponents();
        MenuItem.Label = "MISSILES";
        MenuItem.OnSelect = () =>
        {
            MenuManager.CheckToSwapMenuArea(typeof(MenuAreaMissiles));
        };
    }
}