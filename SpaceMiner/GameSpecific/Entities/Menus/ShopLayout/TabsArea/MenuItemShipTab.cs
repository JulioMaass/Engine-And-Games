using Engine.Managers;
using Engine.Managers.Graphics;
using SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.ShipArea;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.TabsArea;

public class MenuItemShipTab : Entity
{
    public MenuItemShipTab()
    {
        AddPointedLabelMenuComponents(StringDrawer.CutePixelFont);
        MenuItem.Label = "SHIP";
        MenuItem.OnSelect = () =>
        {
            MenuManager.CheckToSwapMenuArea(typeof(MenuAreaShip));
        };
    }
}