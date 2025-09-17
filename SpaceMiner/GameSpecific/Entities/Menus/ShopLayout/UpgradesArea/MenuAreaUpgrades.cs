using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.UpgradesArea;

public class MenuAreaUpgrades : MenuArea
{
    public MenuAreaUpgrades()
    {
        MenuItemTypes = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemSocketBlue), typeof(MenuItemSocketYellow), typeof(MenuItemSocketGreen), typeof(MenuItemSocketRed) },
        });
        Position = IntVector2.New(64 + 16, 64 + 12);
        Spacing = IntVector2.New(64 + 32, 32 + 32);
    }
}
