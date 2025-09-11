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
            { typeof(MenuItemRepair), typeof(MenuItemMaxHp), typeof(MenuItemDefense), typeof(MenuItemSpeed) },
            { typeof(MenuItemDash), typeof(MenuItemMissileCapacity), typeof(MenuItemMagnet), typeof(MenuItemSpeed) },
        });
        Position = IntVector2.New(64 + 16, 64 + 16);
        Spacing = IntVector2.New(64 + 32, 32 + 32);
    }
}
