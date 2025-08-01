using Engine.Managers;
using SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.WeaponsArea;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.TabsArea;

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