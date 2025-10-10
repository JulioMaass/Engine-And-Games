using Engine.Managers;
using Engine.Managers.Graphics;
using SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.WeaponsArea;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.TabsArea;

public class MenuItemWeaponTab : Entity
{
    public MenuItemWeaponTab()
    {
        AddPointedLabelMenuComponents(StringDrawer.CutePixelFont);
        MenuItem.Label = "WEAPONS";
        MenuItem.OnSelect = () =>
        {
            MenuManager.CheckToSwapMenuArea(typeof(MenuAreaWeapons));
        };
    }
}