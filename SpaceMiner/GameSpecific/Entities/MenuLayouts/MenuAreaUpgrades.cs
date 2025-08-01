using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Types;
using SpaceMiner.GameSpecific.Entities.MenuItems;

namespace SpaceMiner.GameSpecific.Entities.MenuLayouts;

public class MenuAreaUpgrades : MenuArea
{
    public MenuAreaUpgrades()
    {
        MenuItemTypes = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemSocketBlue), typeof(MenuItemSocketYellow), typeof(MenuItemSocketGreen), typeof(MenuItemSocketRed) },
        });
        Position = IntVector2.New(64, 64);
        Spacing = IntVector2.New(64 + 32, 32 + 32);
    }
}
