using Engine.ECS.Components.MenuHandling;
using Engine.Helpers;
using Engine.Types;

namespace SpaceMiner.GameSpecific.Entities.Menus.ShopLayout.WeaponsArea;

public class MenuAreaWeapons : MenuArea
{
    public MenuAreaWeapons()
    {
        MenuItemTypes = Extensions.NewTransposedArray(new[,]
        {
            { typeof(MenuItemBasicShot), typeof(MenuItemMachineGun), typeof(MenuItemShotgun), typeof(MenuItemSlugger), typeof(MenuItemBlaster), typeof(MenuItemWarper) },
            { typeof(MenuItemPuncher), null, null, null, null, null },
            { null, null, null, null, null, null },
        });
        Position = IntVector2.New(64 + 16, 64 - 4);
        Spacing = IntVector2.New(64, 32 + 32);
    }
}
