
namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemWeaponGas : Entity
{
    public MenuItemWeaponGas()
    {
        AddWeaponMenuItemComponents();
        AddHudSprite("WeaponIconsGas", 16, 16, 0, 0);
        MenuItem.Label = "I.FLAME       S.CLOUD  ";
    }
}