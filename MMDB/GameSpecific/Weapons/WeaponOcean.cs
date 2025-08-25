using Engine.ECS.Components.ShootingHandling;

namespace MMDB.GameSpecific.Weapons;

public class WeaponOcean : Weapon
{
    public WeaponOcean(Entity owner)
    {
        Owner = owner;
        PaletteId = 16;
    }
}
