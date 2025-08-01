using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.MenuItems;

namespace SpaceMiner.GameSpecific.Entities.MenuLayouts;

public class MenuAreaTabs : MenuArea
{
    public MenuAreaTabs()
    {
        MenuItemTypes = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemMissileTab), typeof(MenuItemWeaponTab), typeof(MenuItemUpgradeTab) },
        });
        Position = IntVector2.New(64, 32);
        Spacing = IntVector2.New(64 + 32, 32 + 32);
    }
}
