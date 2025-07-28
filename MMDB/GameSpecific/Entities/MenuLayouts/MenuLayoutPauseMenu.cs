using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Managers.Graphics;
using Engine.Types;
using MMDB.GameSpecific.Entities.MenuItems;

namespace MMDB.GameSpecific.Entities.MenuLayouts;

public class MenuLayoutPauseMenu : MenuLayout
{
    public MenuLayoutPauseMenu()
    {
        BackgroundImage = Drawer.TextureDictionary["PauseMenu"];

        var array = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemWeaponBuster), },
            { typeof(MenuItemWeaponChain), },
            { typeof(MenuItemWeaponGas), },
            { typeof(MenuItemWeaponPolice), },
            { typeof(MenuItemWeaponGhost), },
            { typeof(MenuItemWeaponOcean), },
            { typeof(MenuItemWeaponVacuum), },
            { typeof(MenuItemWeaponJungle), },
            { typeof(MenuItemWeaponRemote), },
        });
        var position = IntVector2.New(91, 14);
        var spacing = IntVector2.New(0, 16);
        var menuArea = new MenuArea(array, position, spacing);
        MenuAreas.Add(menuArea);
    }
}