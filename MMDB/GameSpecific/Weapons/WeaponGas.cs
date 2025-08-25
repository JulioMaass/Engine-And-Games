using Engine.ECS.Components.ShootingHandling;

namespace MMDB.GameSpecific.Weapons;

public class WeaponGas : Weapon
{
    public WeaponGas(Entity owner)
    {
        Owner = owner;
        PaletteId = 9;
    }
}
