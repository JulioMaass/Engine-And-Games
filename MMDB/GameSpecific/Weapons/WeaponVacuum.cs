using Engine.ECS.Components.ShootingHandling;

namespace MMDB.GameSpecific.Weapons;

public class WeaponVacuum : Weapon
{
    public WeaponVacuum(Entity owner)
    {
        Owner = owner;
        PaletteId = 17;
    }
}
