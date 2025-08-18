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
            { typeof(MenuItemBasicShot), typeof(MenuItemMachineGun), typeof(MenuItemShotgun), typeof(MenuItemSlugger) },
            { typeof(MenuItemPuncher), typeof(MenuItemWarper), typeof(MenuItemBlaster), typeof(MenuItemWarper) },
        });
        Position = IntVector2.New(64, 64);
        Spacing = IntVector2.New(64 + 32, 32 + 32);
    }
}
