using Engine.ECS.Components.ShootingHandling;

namespace MMDB.GameSpecific.Weapons;

public class WeaponPolice : Weapon
{
    public WeaponPolice(Entity owner)
    {
        Owner = owner;
        PaletteId = 10;
    }
}
