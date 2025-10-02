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
            { typeof(MenuItemSocketBlueAttackSpeed), typeof(MenuItemSocketYellowStraightMulti), typeof(MenuItemSocketGreenDamage), typeof(MenuItemSocketRed), null, null },
            { typeof(MenuItemSocketBlueShotSpeed), typeof(MenuItemSocketYellowAngleMulti), typeof(MenuItemSocketGreenPierce), null, null, null },
            { typeof(MenuItemSocketBlueDuration), typeof(MenuItemSocketYellowSplit), typeof(MenuItemSocketGreenSize), null, null, null },
        });
        Position = IntVector2.New(64 + 16, 64 - 4);
        Spacing = IntVector2.New(64, 32 + 32);
    }
}
