using Engine.ECS.Components.MenuHandling;
using Engine.Managers.Graphics;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.MissilesArea;
using SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.OptionsArea;
using SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.TabsArea;
using SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;
using SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.WeaponsArea;
using System;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout;

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

        SwappableAreaTypes.Add(typeof(MenuAreaMissiles));
        SwappableAreaTypes.Add(typeof(MenuAreaWeapons));
        SwappableAreaTypes.Add(typeof(MenuAreaUpgrades));
    }
}