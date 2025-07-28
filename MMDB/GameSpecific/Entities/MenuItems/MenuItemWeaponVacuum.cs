
namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemWeaponVacuum : Entity
{
    public MenuItemWeaponVacuum()
    {
        AddWeaponMenuItemComponents();
        AddHudSprite("WeaponIconsVacuum", 16, 16, 0, 0);
        MenuItem.Label = "J.SHOT        V.CRUSH  ";
    }
}