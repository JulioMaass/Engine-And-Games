
namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemWeaponJungle : Entity
{
    public MenuItemWeaponJungle()
    {
        AddWeaponMenuItemComponents();
        AddHudSprite("WeaponIconsJungle", 16, 16, 0, 0);
        MenuItem.Label = "W.SLASH       J.SEED   ";
    }
}