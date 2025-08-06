using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.MissilesArea;

public class MenuAreaMissiles : MenuArea
{
    public MenuAreaMissiles()
    {
        MenuItemTypes = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemMissileSpray), typeof(MenuItemMissileDrill), typeof(MenuItemMissileAtomic), typeof(MenuItemMissileHoming) },
        });
        Position = IntVector2.New(64, 64);
        Spacing = IntVector2.New(64 + 32, 32 + 32);
    }
}
