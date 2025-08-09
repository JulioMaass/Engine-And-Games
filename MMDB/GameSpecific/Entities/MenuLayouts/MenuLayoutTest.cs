using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Types;
using MMDB.GameSpecific.Entities.MenuItems;

namespace MMDB.GameSpecific.Entities.MenuLayouts;

public class MenuLayoutTest : MenuLayout
{
    public MenuLayoutTest()
    {
        var arrayTop = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemStageIntro), typeof(MenuItemLabelMain), },
            { typeof(MenuItemLabelOptions), typeof(MenuItemLabelControls), },
            { typeof(MenuItemLabelTest), typeof(MenuItemLabelVisuals), }
        });
        var positionTop = IntVector2.New(100, 100);
        var spacingTop = IntVector2.New(100, 10);
        var menuAreaTop = new MenuArea(arrayTop, positionTop, spacingTop);
        MenuAreas.Add(menuAreaTop);

        var arrayDownLeft = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemStageIntro), typeof(MenuItemLabelMain), },
            { typeof(MenuItemLabelOptions), typeof(MenuItemLabelControls), },
            { typeof(MenuItemLabelTest), typeof(MenuItemLabelVisuals), }
        });
        var positionDownLeft = IntVector2.New(100, 150);
        var spacingDownLeft = IntVector2.New(50, 10);
        var menuAreaDownLeft = new MenuArea(arrayDownLeft, positionDownLeft, spacingDownLeft);
        MenuAreas.Add(menuAreaDownLeft);

        var arrayDownRight = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemStageIntro), typeof(MenuItemLabelMain), },
            { typeof(MenuItemLabelOptions), typeof(MenuItemLabelControls), },
            { typeof(MenuItemLabelTest), typeof(MenuItemLabelVisuals), }
        });
        var positionDownRight = IntVector2.New(200, 150);
        var spacingDownRight = IntVector2.New(50, 10);
        var menuAreaDownRight = new MenuArea(arrayDownRight, positionDownRight, spacingDownRight);
        MenuAreas.Add(menuAreaDownRight);
    }
}