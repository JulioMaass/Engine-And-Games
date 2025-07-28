
namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemWeaponPolice : Entity
{
    public MenuItemWeaponPolice()
    {
        AddWeaponMenuItemComponents();
        AddHudSprite("WeaponIconsPolice", 16, 16, 0, 0);
        MenuItem.Label = "L.PISTOL      F.SHOUT  ";
    }
}