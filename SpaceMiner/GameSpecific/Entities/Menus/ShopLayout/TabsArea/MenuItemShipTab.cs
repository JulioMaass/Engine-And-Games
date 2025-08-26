using Engine.Managers;
using SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.ShipArea;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.TabsArea;

public class MenuItemShipTab : Entity
{
    public MenuItemShipTab()
    {
        AddPointedLabelMenuComponents();
        MenuItem.Label = "SHIP";
        MenuItem.OnSelect = () =>
        {
            MenuManager.CheckToSwapMenuArea(typeof(MenuAreaShip));
        };
    }
}