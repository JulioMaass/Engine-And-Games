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
            { typeof(MenuItemMissileSpray), typeof(MenuItemMissileDrill), typeof(MenuItemMissileAtomic), typeof(MenuItemMissileHoming), typeof(MenuItemMissileMine), null },
            { null, null, null, null, null, null },
            { null, null, null, null, null, null },
        });
        Position = IntVector2.New(64 + 16, 64 - 4);
        Spacing = IntVector2.New(64, 32 + 32);
    }
}
