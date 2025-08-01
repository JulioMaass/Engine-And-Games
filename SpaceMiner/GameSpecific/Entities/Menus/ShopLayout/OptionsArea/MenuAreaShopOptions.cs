using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.OptionsArea;

public class MenuAreaShopOptions : MenuArea
{
    public MenuAreaShopOptions()
    {
        MenuItemTypes = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemUpgradesExit) },
        });
        Position = IntVector2.New(256 - 64, 128 + 64);
        Spacing = IntVector2.New(64, 32);
    }
}
