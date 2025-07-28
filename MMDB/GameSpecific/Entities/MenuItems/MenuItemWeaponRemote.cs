
namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemWeaponRemote : Entity
{
    public MenuItemWeaponRemote()
    {
        AddWeaponMenuItemComponents();
        AddHudSprite("WeaponIconsRemote", 16, 16, 0, 0);
        MenuItem.Label = "R.CUTTER      P.CRASHER";
    }
}