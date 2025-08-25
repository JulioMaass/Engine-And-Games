using Engine.ECS.Components.ShootingHandling;

namespace MMDB.GameSpecific.Weapons;

public class WeaponChain : Weapon
{
    public WeaponChain(Entity owner)
    {
        Owner = owner;
        PaletteId = 8;
    }
}
