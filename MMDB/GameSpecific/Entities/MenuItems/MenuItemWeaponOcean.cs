
namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemWeaponOcean : Entity
{
    public MenuItemWeaponOcean()
    {
        AddWeaponMenuItemComponents();
        AddHudSprite("WeaponIconsOcean", 16, 16, 0, 0);
        MenuItem.Label = "T.TORPEDO     O.FLUSH  ";
    }
}