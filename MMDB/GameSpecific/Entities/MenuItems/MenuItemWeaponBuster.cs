
namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemWeaponBuster : Entity
{
    public MenuItemWeaponBuster()
    {
        AddWeaponMenuItemComponents();
        AddHudSprite("WeaponIconsBuster", 16, 16, 0, 0);
        MenuItem.Label = "M.BUSTER      DOUBLE B.";
    }
}