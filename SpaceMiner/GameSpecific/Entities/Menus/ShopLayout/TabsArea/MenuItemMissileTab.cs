using Engine.Managers;
using Engine.Managers.Graphics;
using SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.MissilesArea;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.TabsArea;

public class MenuItemMissileTab : Entity
{
    public MenuItemMissileTab()
    {
        AddPointedLabelMenuComponents(StringDrawer.CutePixelFont);
        MenuItem.Label = "MISSILES";
        MenuItem.OnSelect = () =>
        {
            MenuManager.CheckToSwapMenuArea(typeof(MenuAreaMissiles));
        };
    }
}