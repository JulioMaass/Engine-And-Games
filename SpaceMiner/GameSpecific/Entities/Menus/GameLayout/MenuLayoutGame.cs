using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.GameLayout;

public class MenuLayoutGame : MenuLayout
{
    public MenuLayoutGame()
    {
        var array = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemPermanentUpgrades) },
            { typeof(MenuItemStageSelect) },
        });
        var position = IntVector2.New(100, 100);
        var spacing = IntVector2.New(20, 10);
        var menuArea = new MenuArea(array, position, spacing);
        MenuAreas.Add(menuArea);
    }
}