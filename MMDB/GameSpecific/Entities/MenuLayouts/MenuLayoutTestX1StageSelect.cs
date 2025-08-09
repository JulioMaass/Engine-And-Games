using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Types;
using MMDB.GameSpecific.Entities.MenuItems;

namespace MMDB.GameSpecific.Entities.MenuLayouts;

public class MenuLayoutTestX1StageSelect : MenuLayout
{
    public MenuLayoutTestX1StageSelect()
    {
        var overallSpacing = IntVector2.New(80, 30);
        var overallPosition = IntVector2.New(50, 100);

        var menuAreaTopLeft = new MenuArea(Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemLabelTest), },
        }),
        overallPosition,
        overallSpacing);
        MenuAreas.Add(menuAreaTopLeft);

        var menuAreaTopCenter = new MenuArea(Extensions.NewTransposedArray(new[,]
            {
                { typeof(MenuItemLabelTest), typeof(MenuItemLabelTest), },
            }),
            overallPosition + (overallSpacing.X, 0),
            overallSpacing);
        MenuAreas.Add(menuAreaTopCenter);

        var menuAreaTopRight = new MenuArea(Extensions.NewTransposedArray(new[,]
            {
                { typeof(MenuItemLabelTest), },
            }),
            overallPosition + (overallSpacing.X * 3, 0),
            overallSpacing);
        MenuAreas.Add(menuAreaTopRight);

        var menuAreaCenterLeft = new MenuArea(Extensions.NewTransposedArray(new[,]
            {
                { typeof(MenuItemLabelTest), },
                { typeof(MenuItemLabelTest), },
            }),
            overallPosition + (0, overallSpacing.Y),
            overallSpacing);
        MenuAreas.Add(menuAreaCenterLeft);

        var menuAreaCenterCenter = new MenuArea(Extensions.NewTransposedArray(new[,]
            {
                { typeof(MenuItemLabelTest), },
            }),
            overallPosition + ((int)(overallSpacing.X * 1.5f), (int)(overallSpacing.Y * 1.5f)),
            overallSpacing);
        MenuAreas.Add(menuAreaCenterCenter);

        var menuAreaCenterRight = new MenuArea(Extensions.NewTransposedArray(new[,]
            {
                { typeof(MenuItemLabelTest), },
                { typeof(MenuItemLabelTest), },
            }),
            overallPosition + (overallSpacing.X * 3, overallSpacing.Y),
            overallSpacing);
        MenuAreas.Add(menuAreaCenterRight);

        var menuAreaBottomLeft = new MenuArea(Extensions.NewTransposedArray(new[,]
            {
                { typeof(MenuItemLabelTest), },
            }),
            overallPosition + (0, overallSpacing.Y * 3),
            overallSpacing);
        MenuAreas.Add(menuAreaBottomLeft);

        var menuAreaBottomCenter = new MenuArea(Extensions.NewTransposedArray(new[,]
            {
                { typeof(MenuItemLabelTest), typeof(MenuItemLabelTest), },
            }),
            overallPosition + (overallSpacing.X, overallSpacing.Y * 3),
            overallSpacing);
        MenuAreas.Add(menuAreaBottomCenter);

        var menuAreaBottomRight = new MenuArea(Extensions.NewTransposedArray(new[,]
            {
                { typeof(MenuItemLabelTest), },
            }),
            overallPosition + (overallSpacing.X * 3, overallSpacing.Y * 3),
            overallSpacing);
        MenuAreas.Add(menuAreaBottomRight);
    }
}