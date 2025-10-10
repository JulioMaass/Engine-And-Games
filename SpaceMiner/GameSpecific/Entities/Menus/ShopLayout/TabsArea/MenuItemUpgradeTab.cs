using Engine.Managers;
using Engine.Managers.Graphics;
using SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.TabsArea;

public class MenuItemUpgradeTab : Entity
{
    public MenuItemUpgradeTab()
    {
        AddPointedLabelMenuComponents(StringDrawer.CutePixelFont);
        MenuItem.Label = "UPGRADES";
        MenuItem.OnSelect = () =>
        {
            MenuManager.CheckToSwapMenuArea(typeof(MenuAreaUpgrades));
        };
    }
}