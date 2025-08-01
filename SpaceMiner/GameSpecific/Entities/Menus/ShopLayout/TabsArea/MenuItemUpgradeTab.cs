using Engine.Managers;
using SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.TabsArea;

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