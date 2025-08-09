using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Managers.Graphics;
using Engine.Types;
using MMDB.GameSpecific.Entities.MenuItems;

namespace MMDB.GameSpecific.Entities.MenuLayouts;

public class MenuLayoutStageSelect : MenuLayout
{
    public MenuLayoutStageSelect()
    {
        BackgroundImage = Drawer.TextureDictionary["StageSelect"];
        StartingCursorItem = typeof(MenuItemStageIntro);

        var array = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemStagePolice), typeof(MenuItemStageChain), typeof(MenuItemStageVacuum), },
            { typeof(MenuItemStageJungle), typeof(MenuItemStageIntro), typeof(MenuItemStageOcean), },
            { typeof(MenuItemStageGas), typeof(MenuItemStageGhost), typeof(MenuItemStageRemote), },
        });
        var position = IntVector2.New(88, 8);
        var spacing = IntVector2.New(80, 64);
        var menuArea = new MenuArea(array, position, spacing);
        MenuAreas.Add(menuArea);
    }
}