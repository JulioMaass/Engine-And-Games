using Engine.ECS.Components.ShootingHandling;

namespace MMDB.GameSpecific.Weapons;

public class WeaponGhost : Weapon
{
    public WeaponGhost(Entity owner)
    {
        Owner = owner;
        PaletteId = 14;
    }
}
