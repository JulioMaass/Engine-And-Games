using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.StageSelectLayout;

public class MenuLayoutStageSelect : MenuLayout
{
    public MenuLayoutStageSelect()
    {
        var array = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemStageBlue) , typeof(MenuItemStageBlue) },
            { typeof(MenuItemStageBlue) , typeof(MenuItemStageBlue) },
        });
        var position = IntVector2.New(100, 100);
        var spacing = IntVector2.New(32, 32);
        var menuArea = new MenuArea(array, position, spacing);
        MenuAreas.Add(menuArea);
    }
}