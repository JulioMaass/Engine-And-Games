using Engine.ECS.Components.MenuHandling;
using Engine.Managers.Graphics;
using Engine.Types;
using System;

namespace SpaceMiner.GameSpecific.Entities.MenuLayouts;

public class MenuLayoutShop : MenuLayout
{
    public MenuLayoutShop()
    {
        BackgroundImage = Drawer.TextureDictionary["BlackTile"];
        BackgroundImageSize = new IntVector2(256 + 64, 128 + 64);
        BackgroundImagePosition = new IntVector2(64, 32);

        var menuAreaTabs = new MenuAreaTabs();
        var menuAreaCurrentTab = (MenuArea)Activator.CreateInstance(typeof(MenuAreaMissiles));
        var menuAreaShopOptions = new MenuAreaShopOptions();
        MenuAreas.Add(menuAreaTabs);
        MenuAreas.Add(menuAreaCurrentTab);
        MenuAreas.Add(menuAreaShopOptions);

        menuAreaTabs.AllowedAreasDown.Add(menuAreaCurrentTab);
        menuAreaCurrentTab.AllowedAreasUp.Add(menuAreaTabs);
        menuAreaCurrentTab.AllowedAreasDown.Add(menuAreaShopOptions);
        menuAreaShopOptions.AllowedAreasUp.Add(menuAreaCurrentTab);

        SwappableAreaTypes.Add(typeof(MenuAreaMissiles));
        SwappableAreaTypes.Add(typeof(MenuAreaWeapons));
        SwappableAreaTypes.Add(typeof(MenuAreaUpgrades));
    }
}