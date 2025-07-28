using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Types;
using ShooterGame.GameSpecific.Entities.MenuItems;

namespace ShooterGame.GameSpecific.Entities.MenuLayouts;

public class MenuLayoutMain : MenuLayout
{
    public MenuLayoutMain()
    {
        var array = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemStageIntro) },
        });
        var position = IntVector2.New(100, 100);
        var spacing = IntVector2.New(20, 10);
        var menuArea = new MenuArea(array, position, spacing);
        MenuAreas.Add(menuArea);
    }
}