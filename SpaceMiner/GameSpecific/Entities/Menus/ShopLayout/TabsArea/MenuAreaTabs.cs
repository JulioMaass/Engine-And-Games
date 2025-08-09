using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.TabsArea;

public class MenuAreaTabs : MenuArea
{
    public MenuAreaTabs()
    {
        MenuItemTypes = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemMissileTab), typeof(MenuItemWeaponTab), typeof(MenuItemUpgradeTab) },
        });
        Position = IntVector2.New(64, 32);
        Spacing = IntVector2.New(64 + 32, 16);
    }
}
