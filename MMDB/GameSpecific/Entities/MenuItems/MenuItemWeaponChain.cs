
namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemWeaponChain : Entity
{
    public MenuItemWeaponChain()
    {
        AddWeaponMenuItemComponents();
        AddHudSprite("WeaponIconsChain", 16, 16, 0, 0);
        MenuItem.Label = "C.CLAW        V.BODY   ";
    }
}