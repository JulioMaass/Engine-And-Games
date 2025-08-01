using Engine.Managers;
using SpaceMiner.GameSpecific.Entities.MenuLayouts;

namespace SpaceMiner.GameSpecific.Entities.MenuItems;

public class MenuItemUpgradeTab : Entity
{
    public MenuItemUpgradeTab()
    {
        AddPointedLabelMenuComponents();
        MenuItem.Label = "UPGRADES";
        MenuItem.OnSelect = () =>
        {
            MenuManager.CheckToSwapMenuArea(typeof(MenuAreaUpgrades));
        };
    }
}