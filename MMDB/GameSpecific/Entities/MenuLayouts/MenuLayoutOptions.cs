using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Types;
using MMDB.GameSpecific.Entities.MenuItems;

namespace MMDB.GameSpecific.Entities.MenuLayouts;

public class MenuLayoutOptions : MenuLayout
{
    public MenuLayoutOptions()
    {
        var array = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemLabelControls) },
            { typeof(MenuItemLabelVisuals) },
        });
        var position = IntVector2.New(100, 100);
        var spacing = IntVector2.New(20, 10);
        var menuArea = new MenuArea(array, position, spacing);
        MenuAreas.Add(menuArea);
    }
}