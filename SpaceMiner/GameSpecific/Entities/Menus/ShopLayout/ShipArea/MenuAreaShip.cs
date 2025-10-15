using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.ShipArea;

public class MenuAreaShip : MenuArea
{
    public MenuAreaShip()
    {
        MenuItemTypes = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemRepair), typeof(MenuItemMaxHp), typeof(MenuItemDefense), typeof(MenuItemSpeed), typeof(MenuItemDash), null },
            { typeof(MenuItemMissileCapacity), typeof(MenuItemMagnet), null, null, null, null },
            { null, null, null, null, null, null },
        });
        Position = IntVector2.New(64 + 16, 64 - 4);
        Spacing = IntVector2.New(64, 32 + 32);
    }
}
