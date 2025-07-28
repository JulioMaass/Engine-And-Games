
namespace MMDB.GameSpecific.Entities.MenuItems;

public class MenuItemWeaponGhost : Entity
{
    public MenuItemWeaponGhost()
    {
        AddWeaponMenuItemComponents();
        AddHudSprite("WeaponIconsGhost", 16, 16, 0, 0);
        MenuItem.Label = "P.WAVE        G.IMAGE  ";
    }
}